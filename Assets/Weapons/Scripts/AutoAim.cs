using UnityEngine;
using System.Collections;
//Parent has to have Shooter implementation!
public class AutoAim : MonoBehaviour {
	enum Mode{
		idle,
		aiming,
		shooting,
	}
	public float time2changeIdle;
	Mode mode;
	Shooter shooter;
	Transform target;
	// Use this for initialization
	void Start () {
		shooter = GetComponentInParent<Shooter> ();
		if (shooter == null) {
			Debug.LogError ("Parent doesn't have Shooter in " + name);
		}
		StartCoroutine (Think ());
	}
	IEnumerator Think(){
		while(true){
			switch (mode) {
			case Mode.idle:
				Vector3 newPos = Random.onUnitSphere * shooter.Range;
				newPos.y = Mathf.Abs(newPos.y);
				Vector3 nowPos = transform.localPosition;
				if (StageManager.Instance.dragonsInDangerZone.Count > 0) {
					mode = Mode.aiming;
					break;
				}
				//yield return new WaitForSeconds (time2changeIdle);
				for (float i = 0f; i < time2changeIdle; i += Time.deltaTime) {
					yield return null;
					transform.localPosition = Vector3.Lerp (nowPos, newPos, i / time2changeIdle);
				}
				break;
			case Mode.aiming:
				float sqrdist;
				target = StageManager.Instance.GetClosestDragonFromDangerZone (transform, out sqrdist);
				if (sqrdist < shooter.Range * shooter.Range) {
					mode = Mode.shooting;
					shooter.shoot = true;
					break;
				}
				transform.position = target.position;
				yield return new WaitForSeconds(0.2f);
				break;
			case Mode.shooting:
				if (target != null && (target.position - transform.position).sqrMagnitude <shooter.Range * shooter.Range) {
					transform.position = target.position;
				} else {
					mode = Mode.aiming;
					break;
				}
				yield return null;
				break;
			}
			if (StageManager.Instance.dragonsInDangerZone.Count == 0) {
				mode = Mode.idle;
				shooter.shoot = false;
			}
		}
	}
}
