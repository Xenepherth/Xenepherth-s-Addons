// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================
using UnityEngine;
using Mirror;
using System;

// =======================================================================================
// ENTITY
// =======================================================================================
public partial class Entity
{
    [Header("[-=-=- UCE ATTRIBUTES -=-=-]")]
    [Tooltip("[Optional] All damage dealt can vary randomly (0.25 = +/- 25%) (0 to disable)")]
    [Range(0, 1)] public float UCE_randomDamageDeviation = 0.25f;

    [Tooltip("[Optional] Check to use new relational damage formula, uncheck to use old (attack-defense=damage) formula. ")]
    public bool UCE_relationalDamage = true;

    // -----------------------------------------------------------------------------------
    // DealDamageAt
    // -----------------------------------------------------------------------------------
    [Server]
    public virtual void DealDamageAt(Entity entity, int amount, float stunChance = 0, float stunTime = 0)
    {
        int damageDealt = 0;
        DamageType damageType = DamageType.Normal;

        if (entity && !entity.combat.invincible && entity.isAlive && (amount > 0 || stunChance > 0 || stunTime > 0))
        {
            int target_Defense = entity.combat.defense - Convert.ToInt32(entity.combat.defense * defenseBreakFactor);
            float target_BlockChance = entity.combat.blockChance - Convert.ToInt32(entity.combat.blockChance * blockBreakFactor);
            float self_critChance = combat.criticalChance - Convert.ToInt32(combat.criticalChance * entity.criticalEvasion);

            // -- Base Damage
            if (UCE_relationalDamage)
                damageDealt = Convert.ToInt32((amount * (100 - Mathf.Sqrt(target_Defense)) / 100));
            else
                damageDealt = Mathf.Max(amount - target_Defense, 1);

            // -- Elemental Modifiers
            UCE_ElementTemplate element = null;

#if _iMMOELEMENTS
            if (damageDealt > 0)
            {
                if (skills.currentSkill != -1 && skills.skills[skills.currentSkill].data is DamageSkill)
                    element = ((DamageSkill)skills.skills[skills.currentSkill].data).element;

#if _iMMOPROJECTILES || _iMMOMELEE
                if (skills.currentSkill != -1 && skills.skills[skills.currentSkill].data is UCE_DamageSkill)
                    element = ((UCE_DamageSkill)skills.skills[skills.currentSkill].data).element;
#endif

                if (element == null && this is Player)
                    element = ((Player)this).UCE_getAttackElement();
            }
#endif

            // -- Custom Hook for Modifiers
            if (damageDealt > 0)
            {
                MutableWrapper<int> damageBonus = new MutableWrapper<int>(0);
                Utils.InvokeMany(typeof(Entity), this, "OnDealDamage_", entity, element, damageDealt, damageBonus);
                damageDealt += damageBonus.Value;
            }

            // -- Randomized Variance
            if (damageDealt > 0 && UCE_randomDamageDeviation != 0)
            {
                int minDamage = (int)UnityEngine.Random.Range((int)damageDealt * (1 - UCE_randomDamageDeviation), damageDealt);
                int maxDamage = (int)UnityEngine.Random.Range(damageDealt, (int)damageDealt * (1 + UCE_randomDamageDeviation));
                damageDealt = UnityEngine.Random.Range(minDamage, maxDamage);
            }

            // -- Block
            if (UnityEngine.Random.value < target_BlockChance)
            {
                damageDealt -= Convert.ToInt32(damageDealt * blockFactor);
                damageType = DamageType.Block;
            }
            else if (resistance <= accuracy)
            {
                // -- Crit
                if (UnityEngine.Random.value < self_critChance)
                {
                    damageDealt = Convert.ToInt32(damageDealt * criticalFactor);
                    damageType = DamageType.Crit;
                }
                // -- Deal Damage
                entity.health.current -= damageDealt;

                // -- Check Stun
                if (UnityEngine.Random.value <= stunChance)
                {
                    double newStunEndTime = NetworkTime.time + stunTime;
                    entity.stunTimeEnd = Math.Max(newStunEndTime, stunTimeEnd);
                }

                // call OnDamageDealtTo / OnKilledEnemy events
                combat.onDamageDealtTo.Invoke(entity);
                if (entity.health.current == 0)
                combat.onKilledEnemy.Invoke(entity);
            }
            else
                damageType = DamageType.Miss;

            entity.OnAggro(this);
            entity.combat.RpcOnReceivedDamaged(damageDealt, damageType);

            // reset last combat time for both
            lastCombatTime = NetworkTime.time;
            entity.lastCombatTime = NetworkTime.time;

            Utils.InvokeMany(typeof(Entity), this, "DealDamageAt_", entity, amount, stunChance, stunTime);
        }
    }

    // -----------------------------------------------------------------------------------
    // DealDamageAt_UCE_Attributes
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("DealDamageAt")]
    private void DealDamageAt_UCE_Attributes(Entity entity, int amount, float stunChance = 0, float stunTime = 0)
    {
        if (entity == null || amount <= 0 || !isAlive || !entity.isAlive) return;

        // ---- Drain Health
        if (drainHealthFactor != 0)
            health.current += (int)(amount * drainHealthFactor);

        // ---- Drain Mana
        if (drainManaFactor != 0)
            mana.current += (int)(amount * drainManaFactor);

        // ---- Reflect Damage
        if (entity.reflectDamageFactor != 0)
            health.current -= (int)(amount * entity.reflectDamageFactor);

        // ---- Absorb Health
        if (entity.absorbHealthFactor != 0)
            entity.health.current += (int)(amount * entity.absorbHealthFactor);

        // ---- Absorb Mana
        if (entity.absorbManaFactor != 0)
            entity.mana.current += (int)(amount * entity.absorbManaFactor);
    }

    // -----------------------------------------------------------------------------------
    // blockFactor
    // -----------------------------------------------------------------------------------
    [Header("Block Factor")]
    [SerializeField] protected LinearFloat _blockFactor;

    public virtual float blockFactor
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusBlockFactor.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusBlockFactor;

            // base + passives + buffs
            return _blockFactor.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // criticalFactor
    // -----------------------------------------------------------------------------------
    [Header("Critical Factor")]
    [SerializeField] protected LinearFloat _criticalFactor;

    public virtual float criticalFactor
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusCriticalFactor.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusCriticalFactor;

            // base + passives + buffs
            return _criticalFactor.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // DrainHealthFactor
    // -----------------------------------------------------------------------------------
    [Header("Drain Health Factor")]
    [SerializeField] protected LinearFloat _drainHealthFactor;

    public virtual float drainHealthFactor
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusDrainHealthFactor.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusDrainHealthFactor;

            // base + passives + buffs
            return _drainHealthFactor.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // DrainManaFactor
    // -----------------------------------------------------------------------------------
    [Header("Drain Mana Factor")]
    [SerializeField] protected LinearFloat _drainManaFactor;

    public virtual float drainManaFactor
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusDrainManaFactor.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusDrainManaFactor;

            // base + passives + buffs
            return _drainManaFactor.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // ReflectDamageFactor
    // -----------------------------------------------------------------------------------
    [Header("Reflect Damage Factor")]
    [SerializeField] protected LinearFloat _reflectDamageFactor;

    public virtual float reflectDamageFactor
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusReflectDamageFactor.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusReflectDamageFactor;

            // base + passives + buffs
            return _reflectDamageFactor.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // DefenseBreakFactor
    // -----------------------------------------------------------------------------------
    [Header("Defense Break Factor")]
    [SerializeField] protected LinearFloat _defenseBreakFactor;

    public virtual float defenseBreakFactor
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusDefenseBreakFactor.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusDefenseBreakFactor;

            // base + passives + buffs
            return _defenseBreakFactor.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // Block Break Factor
    // -----------------------------------------------------------------------------------
    [Header("Block Break Factor")]
    [SerializeField] protected LinearFloat _blockBreakFactor;

    public virtual float blockBreakFactor
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusBlockBreakFactor.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusBlockBreakFactor;

            // base + passives + buffs
            return _blockBreakFactor.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // CriticalEvasion
    // -----------------------------------------------------------------------------------
    [Header("Critical Evasion")]
    [SerializeField] protected LinearFloat _criticalEvasion;

    public virtual float criticalEvasion
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusCriticalEvasion.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusCriticalEvasion;

            // base + passives + buffs
            return _criticalEvasion.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // Accuracy
    // -----------------------------------------------------------------------------------
    [Header("Accuracy")]
    [SerializeField] protected LinearFloat _accuracy;

    public virtual float accuracy
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusAccuracy.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusAccuracy;

            // base + passives + buffs
            return _accuracy.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // Resistance
    // -----------------------------------------------------------------------------------
    [Header("Resistance")]
    [SerializeField] protected LinearFloat _resistance;

    public virtual float resistance
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusResistance.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusResistance;

            // base + passives + buffs
            return _resistance.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // AbsorbHealthFactor
    // -----------------------------------------------------------------------------------
    [Header("Absorb Health Factor")]
    [SerializeField] protected LinearFloat _absorbHealthFactor;

    public virtual float absorbHealthFactor
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusAbsorbHealthFactor.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusAbsorbHealthFactor;

            // base + passives + buffs
            return _absorbHealthFactor.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // AbsorbManaFactor
    // -----------------------------------------------------------------------------------
    [Header("Absorb Mana Factor")]
    [SerializeField] protected LinearFloat _absorbManaFactor;

    public virtual float absorbManaFactor
    {
        get
        {
            float passiveBonus = 0;
            foreach (Skill skill in skills.skills)
                if (skill.level > 0 && skill.data is PassiveSkill)
                    passiveBonus += ((PassiveSkill)skill.data).bonusAbsorbManaFactor.Get(skill.level);

            float buffBonus = 0;
            foreach (Buff buff in skills.buffs)
                buffBonus += buff.bonusAbsorbManaFactor;

            // base + passives + buffs
            return _absorbManaFactor.Get(level.current) + passiveBonus + buffBonus;
        }
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================