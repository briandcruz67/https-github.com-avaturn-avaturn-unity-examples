using UnityEngine;

namespace BKUnity
{
	public class HumanoidAvatarBuilder : MonoBehaviour
	{
		private Animator _Animator;
		public GameObject root;

		public void Awake()
		{
			if (!root) _Animator = GetComponent<Animator>();
			HumanDescription description = AvatarUtils.CreateHumanDescription(root ? root : gameObject);
			Avatar avatar = AvatarBuilder.BuildHumanAvatar(root ? root : gameObject, description);
			avatar.name = root ? root.name : gameObject.name;
			if (!root) _Animator.avatar = avatar;
		}
	}
}