using Ajax.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace Ajax.Components
{
    public class ComponentAudio : IComponent
    {
        Vector3 sourcePosition;
        Vector3 sourceVelocity;
        float deltaTime;
        float dopplerFactor;
        int audioSource = 0;
        int audioBuffer = 0;

        public ComponentAudio(string audioFile, bool isLooping, bool playImmediately, int pDopplerFactor, float deltaTime)
        {
            audioBuffer = ResourceManager.LoadAudio(audioFile);
            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); // attach the buffer to a source
            AL.Source(audioSource, ALSourceb.Looping, isLooping); // source loops infinitely
            sourcePosition = new Vector3(Position); // give the source a position
            AL.Source(audioSource, ALSource3f.Position, ref sourcePosition);

            if (playImmediately)
            {
                AL.SourcePlay(audioSource); // play the audio source
            }

            DopplerFactor = pDopplerFactor;
            Velocity = new Vector3(sourceVelocity);
            this.deltaTime = deltaTime;
        }

        public Vector3 Position
        {
            get { return sourcePosition; }
            set { sourcePosition += sourceVelocity * deltaTime; }
        }

        public Vector3 Velocity
        {
            get { return sourceVelocity; }
            set { sourceVelocity = value; }
        }

        public float DopplerFactor
        {
            get { return dopplerFactor; }
            set { dopplerFactor = value; }
        }

        public int Source
        {
            get { return audioSource; }
            set { audioSource = value; }
        }

        public int Buffer
        {
            get { return audioBuffer; }
            set { audioBuffer = value; }
        }

        public void Play()
        {
            AL.SourcePlay(audioSource);
        }

        public void Stop()
        {
            AL.SourceStop(audioSource);
        }

        public void Close()
        {
            Stop();
            AL.DeleteSource(audioSource);
            AL.DeleteBuffer(audioBuffer);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
    }
}
