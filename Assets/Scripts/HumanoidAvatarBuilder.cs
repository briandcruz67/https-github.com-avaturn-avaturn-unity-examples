using System;
using UnityEngine;

namespace BKUnity
{
	public static class HumanoidAvatarBuilder
	{
		public static Avatar Build(GameObject root)
		{
			HumanDescription description = AvatarUtils.CreateHumanDescription(root);
			Avatar avatar = AvatarBuilder.BuildHumanAvatar(root.gameObject, description);
			avatar.name = root.name;
			return avatar;
		}
	}
}