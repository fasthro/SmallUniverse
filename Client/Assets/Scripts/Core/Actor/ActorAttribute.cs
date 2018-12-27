using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
	public enum ActorAttributeType
	{
		MoveSpeed,
		AttackSpeed,
		RotationSpeed,
		Range,
		Attack,
		MagicAttack,
		Defense,
		MagicDefense,
		Hp,
		HpMax,
	}

    public class ActorAttribute
    {
		private float m_moveSpeed;
		private float m_attackSpeed;
		private float m_rotationSpeed;
		private float m_range;
		private float m_attack;
		private float m_magicAttack;
		private float m_defense;
		private float m_magicDefense;
		private float m_hp;
		private float m_hpMax;

		public static ActorAttribute Create()
		{
			return new ActorAttribute();
		}

		public float GetAttribute(ActorAttributeType attributeType)
		{
			switch(attributeType)
			{
				case ActorAttributeType.MoveSpeed:
					return m_moveSpeed;
				case ActorAttributeType.AttackSpeed:
					return m_attackSpeed;
				case ActorAttributeType.RotationSpeed:
					return m_rotationSpeed;
				case ActorAttributeType.Range:
					return m_range;
				case ActorAttributeType.Attack:
					return m_attack;
				case ActorAttributeType.MagicAttack:
					return m_magicAttack;
				case ActorAttributeType.Defense:
					return m_defense;
				case ActorAttributeType.MagicDefense:
					return m_magicDefense;
				case ActorAttributeType.Hp:
					return m_hp;
				case ActorAttributeType.HpMax:
					return m_hpMax;
				default:
					return 0;
			}
		}

		public void SetAttribute(ActorAttributeType attributeType, float value)
		{
			switch(attributeType)
			{
				case ActorAttributeType.MoveSpeed:
					m_moveSpeed = value;
					break;
				case ActorAttributeType.AttackSpeed:
					m_attackSpeed = value;
					break;
				case ActorAttributeType.RotationSpeed:
					m_rotationSpeed = value;
					break;
				case ActorAttributeType.Range:
					m_range = value;
					break;
				case ActorAttributeType.Attack:
					m_attack = value;
					break;
				case ActorAttributeType.MagicAttack:
					m_magicAttack = value;
					break;
				case ActorAttributeType.Defense:
					m_defense = value;
					break;
				case ActorAttributeType.MagicDefense:
					m_magicDefense = value;
					break;
				case ActorAttributeType.Hp:
					m_hp = value;
					break;
				case ActorAttributeType.HpMax:
					m_hpMax = value;
					break;
			}
		}
    }
}
