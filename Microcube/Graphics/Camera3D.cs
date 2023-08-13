using Silk.NET.Maths;

namespace Microcube.Graphics
{
    public class Camera3D
    {
        private Vector3D<float> position;
        private Vector3D<float> target;
        private Vector3D<float> intermediatePosition;
        private Vector3D<float> intermediateTarget;

        public Vector3D<float> Position
        {
            get => intermediatePosition;
            set => position = value;
        }

        public Vector3D<float> Target
        {
            get => intermediateTarget;
            set => target = value;
        }

        public float FieldOfView { get; set; }

        public float AspectRatio { get; set; }

        public float MovingSpeed { get; set; }

        public Camera3D(Vector3D<float> position, Vector3D<float> target, float fieldOfView, float aspectRatio, float movingSpeed = 0.0f)
        {
            Position = position;
            Target = target;
            FieldOfView = fieldOfView;
            AspectRatio = aspectRatio;
            MovingSpeed = movingSpeed;
        }

        public Matrix4X4<float> GetProjectionMatrix()
        {
            return Matrix4X4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, 0.1f, 100.0f);
        }

        public Matrix4X4<float> GetViewMatrix()
        {
            return Matrix4X4.CreateLookAt(Position, Target, Vector3D<float>.UnitY);
        }

        public void Update(float deltaTime)
        {
            if (MovingSpeed > 0.0f)
            {
                intermediatePosition += (position - intermediatePosition) * MovingSpeed * deltaTime;
                intermediateTarget += (target - intermediateTarget) * MovingSpeed * deltaTime;
            }
            else
            {
                intermediatePosition = position;
                intermediateTarget = target;
            }
        }
    }
}