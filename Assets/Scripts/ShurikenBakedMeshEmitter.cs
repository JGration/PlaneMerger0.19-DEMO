using UnityEngine;
using System.Collections;

public class ShurikenBakedMeshEmitter : MonoBehaviour {
	[Tooltip("パーティクルで使用したいメッシュ")]
	public SkinnedMeshRenderer skin;
	Mesh baked;

	ParticleSystem particle;
	ParticleSystemRenderer render;
	[Tooltip("パーティクルの再生/停止")]
	public bool emit;
	[Tooltip("パーティクルを放出する間隔（秒）")]
	public float coolDown = 0.5f;
	float interval = 0;

	void Start () {
        if (!skin)
			this.enabled = false;
		particle = GetComponent<ParticleSystem>();
		render = GetComponent<ParticleSystemRenderer>();
	}
	
	void Update () {
		if(emit){
			interval -= Time.deltaTime;
			if(interval < 0){
				GameObject newEmitter = Instantiate(gameObject, transform.position, transform.rotation) as GameObject;
				newEmitter.GetComponent<ShurikenBakedMeshEmitter>().EmitMesh();
				interval = coolDown;
			}
		}else{
			interval = coolDown;
		}

    }

	public void EmitMesh () {
		emit = false;
		baked = new Mesh();
		skin.BakeMesh(baked);
		particle = GetComponent<ParticleSystem>();
		render = GetComponent<ParticleSystemRenderer>();
		render.mesh = baked;
		particle.Play();
		Destroy(gameObject, particle.main.duration);
	}

}