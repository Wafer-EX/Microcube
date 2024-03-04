using System.Numerics;

namespace Microcube.Graphics
{
    /// <summary>
    /// Represents a 3D camera that will look at target. Can move slowly.
    /// </summary>
    public class Camera3D
    {
        private Vector3 _position;
        private Vector3 _target;
        private Vector3 _intermediatePosition;
        private Vector3 _intermediateTarget;

        /// <summary>
        /// Position of the camera.
        /// </summary>
        public Vector3 Position
        {
            get => _intermediatePosition;
            set => _position = value;
        }

        /// <summary>
        /// Position of the target.
        /// </summary>
        public Vector3 Target
        {
            get => _intermediateTarget;
            set => _target = value;
        }

        /// <summary>
        /// Field of view of the camera.
        /// </summary>
        public float FieldOfView { get; set; }

        /// <summary>
        /// Aspect ratio of the camera.
        /// </summary>
        public float AspectRatio { get; set; }

        /// <summary>
        /// Moving speed of the camera. If zero, camera moves instantly.
        /// </summary>
        public float MovingSpeed { get; set; }

        public Camera3D(Vector3 position, Vector3 target, float fieldOfView, float aspectRatio, float movingSpeed = 0.0f)
        {
            Position = position;
            Target = target;
            FieldOfView = fieldOfView;
            AspectRatio = aspectRatio;
            MovingSpeed = movingSpeed;
        }

        /// <summary>
        /// Get projection matrix to use it in a shader.
        /// </summary>
        /// <returns>Projection matrix</returns>
        public Matrix4x4 GetProjectionMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, 0.1f, 100.0f);
        }

        /// <summary>
        /// Get view matrix to use it in a shader.
        /// </summary>
        /// <returns>View matrix</returns>
        public Matrix4x4 GetViewMatrix()
        {
            return Matrix4x4.CreateLookAt(Position, Target, Vector3.UnitY);
        }

        /// <summary>
        /// Updates the camera using moving speed.
        /// </summary>
        /// <param name="deltaTime">Time of the frame.</param>
        public void Update(float deltaTime)
        {
            if (MovingSpeed > 0.0f)
            {
                _intermediatePosition += (_position - _intermediatePosition) * MovingSpeed * deltaTime;
                _intermediateTarget += (_target - _intermediateTarget) * MovingSpeed * deltaTime;
            }
            else
            {
                _intermediatePosition = _position;
                _intermediateTarget = _target;
            }
        }
    }
}