using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =======================================================================================
// ENTITY
// =======================================================================================
public partial class Entity
{
    [Header("-=-=- UCE ELEMENTAL RESISTANCES -=-=-")]
    public LevelBasedElement[] elementalResistances;

    protected Dictionary<string, UCE_ElementCache> _elementsCache;

    // -----------------------------------------------------------------------------------
    // Start_UCE_Elements
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("Start")]
    public void Start_UCE_Elements()
    {
        _elementsCache = new Dictionary<string, UCE_ElementCache>();
    }

    // -----------------------------------------------------------------------------------
    // OnDealDamage_UCE_Elements
    // -----------------------------------------------------------------------------------
    /// <summary>
    //[DevExtMethods("OnDealDamage")]
    /// </summary>
    /// <param name="target"></param>
    /// <param name="element"></param>
    /// <param name="damageDealt"></param>
    /// <param name="damageBonus"></param>
    public void OnDealDamage_UCE_Elements(Entity target, UCE_ElementTemplate element, int damageDealt, MutableWrapper<int> damageBonus)
    {
        if (target == null || element == null || damageDealt <= 0) return;

        damageBonus.Value += (int)(damageDealt * target.UCE_CalculateElementalResistance(element));
    }

    // -----------------------------------------------------------------------------------
    // UCE_CalculateElementalResistance
    // -----------------------------------------------------------------------------------
    public virtual float UCE_CalculateElementalResistance(UCE_ElementTemplate element, bool bCache = true)
    {
        float fResistance = 1.0f;                                       // 1.0f = 100% damage by default
        float fValue = 0f;
        UCE_ElementCache elementCache = null;

        // -- Check Caching
        if (bCache && _elementsCache.TryGetValue(element.name, out elementCache) && Time.time < elementCache.timer)
            return elementCache.value;

        // ------------------------------- Calculation -----------------------------------

        // -- Bonus: Base Resistance
        float fBase = fResistance;                                  // 1.0f = 100% damage by default

        foreach (LevelBasedElement ele in elementalResistances)
        {
            if (ele.template == element)
                fBase += ele.Get(level.current);
        }

        // -- Bonus: Passive Skills
        float fPassiveBonus = (from skill in skills.skills
                               where skill.level > 0 && skill.data is PassiveSkill
                               select ((PassiveSkill)skill.data).GetResistance(element, skill.level)).Sum();

        // -- Bonus: Buffs
        float fBuffBonus = skills.buffs.Sum(buff => buff.GetResistance(element));

        fValue = fResistance - (fBase + fPassiveBonus + fBuffBonus);

        // ----------------------------- Calculation End ---------------------------------

        // -- Update Caching
        if (bCache && elementCache != null)
        {
            elementCache.timer = Time.time + cacheTimerInterval;
            elementCache.value = fValue;
            _elementsCache[element.name] = elementCache;
        }
        else if (bCache)
        {
            elementCache = new UCE_ElementCache();
            elementCache.timer = Time.time + cacheTimerInterval;
            elementCache.value = fValue;
            _elementsCache.Add(element.name, elementCache);
        }

        return fValue;
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================