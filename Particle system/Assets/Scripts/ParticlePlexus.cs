using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class ParticlePlexus : MonoBehaviour
{

    public float maxDistance = 1.0f;
    public int maxConnections = 5;
    public int maxLines = 100;

    new ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;
    ParticleSystem.MainModule particleSystemMainModule;

    public LineRenderer line;
    List<LineRenderer> lines = new List<LineRenderer>();
    Transform _transform;
    // Use this for initialization
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystemMainModule = particleSystem.main;

        _transform = transform;

    }

    private void LateUpdate()
    {
        int maxParticles = particleSystemMainModule.maxParticles;

        if(particles == null || particles.Length < maxParticles)
        {
            particles = new ParticleSystem.Particle[maxParticles];
        }

        int lineIndex = 0;
        int lineCount = lines.Count;

        if(lineCount > maxLines)
        {
            for (int i = maxLines; i < lineCount; i++)
            {
                Destroy(lines[i].gameObject);
            }
            lines.RemoveRange(maxLines, lineCount - maxLines);
            lineCount -= lineCount - maxLines;
        }

        if (maxConnections > 0 && maxLines > 0)
        {


            particleSystem.GetParticles(particles);
            int particleCount = particleSystem.particleCount;
            float maxDisctanceSqr = maxDistance * maxDistance;

            // int lineCount = lines.Count;

            switch (particleSystemMainModule.simulationSpace)
            {
                case ParticleSystemSimulationSpace.Local:
                    {
                        _transform = transform;
                        line.useWorldSpace = false;
                        break;
                    }
                case ParticleSystemSimulationSpace.Custom:
                    {
                        _transform = transform;
                        _transform = particleSystemMainModule.customSimulationSpace;
                        line.useWorldSpace = false;
                        break;
                    }
                case ParticleSystemSimulationSpace.World:
                    {
                        _transform = transform;
                        line.useWorldSpace = true;
                        break;
                    }

                default:
                    {
                        throw new System.NotSupportedException(
                            string.Format("Unsupported simulation space '{0}'.",
                            System.Enum.GetName(typeof(ParticleSystemSimulationSpace), particleSystemMainModule.simulationSpace)));
                    }
            }

            for (int i = 0; i < particleCount; i++)
            {
                int connections = 0;
                if (connections == maxConnections || lineIndex == maxLines)
                {
                    break;
                }

                Vector3 firstParticlePosition = particles[i].position;
               //nt connections = 0;
                for (int j = i + 1; j < particleCount; j++)
                {
                    Vector3 secondParticlePosition = particles[j].position;
                    float distanceSqr = Vector3.SqrMagnitude(firstParticlePosition - secondParticlePosition); // for saving resources. Can use Distance -> more resourse taking;

                    if (distanceSqr <= maxDistance)
                    {
                        LineRenderer lr;
                        if (lineIndex == lineCount)
                        {
                            lr = Instantiate(line, _transform, false);
                            lines.Add(lr);
                            lineCount++;
                        }
                        lr = lines[lineIndex];
                        lines[i].enabled = true;
                        lr.SetPosition(0, firstParticlePosition);
                        lr.SetPosition(1, secondParticlePosition);

                        lineIndex++;
                        connections++;

                        if(connections == maxConnections || lineIndex == maxLines)
                        {
                            break;
                        }
                    }
                }
            }
        }

        for(int i = lineIndex; i < lines.Count; i++)
        {
            lines[i].enabled = false;
        }
    }
}
