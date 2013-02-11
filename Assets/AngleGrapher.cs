using UnityEngine;
using System.Collections;
     
public class AngleGrapher : MonoBehaviour
{
	ParticleSystem.Particle[] cloud;
	bool bPointsUpdated = false;
     
	void Start ()
	{
		SetPoints();
	}
     
	void Update ()
	{
		if (bPointsUpdated) 
		{
			particleSystem.SetParticles(cloud, cloud.Length);
			bPointsUpdated = false;
		}
	}
     
	public void SetPoints (Vector3[] positions, Color[] colors)
	{
		cloud = new ParticleSystem.Particle[positions.Length];
     
		for (int i = 0; i < positions.Length; ++i) {
			cloud [i].position = positions [i];
			cloud [i].color = colors [i];
			cloud [i].size = 0.1f;
		}
     
		bPointsUpdated = true;
	}
}