﻿using fun.Basics.Shapes;
using fun.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using Environment = fun.Core.Environment;

namespace fun.Basics
{
    [Serializable]
    public sealed class CollidingElement : Element
    {
        private static float GAP = 0.0001f;

        private Vector3 lastPos;
        private Vector3 currPos;
        private TransformElement transform;
        private List<ICollider> colliders;

		public bool IsCollidingX {
			get;
            private set;
		}

		public bool IsCollidingY {
			get;
            private set;
		}
			
		public bool IsCollidingZ {
			get;
			private set;
		}

        public CollidingElement(Environment environment, Entity entity)
            : base(environment, entity)
        {
            transform = entity.GetElement<TransformElement>();

            currPos = transform.Position;
            lastPos = transform.Position;
        }

        public override void Initialize()
        {
            var elements = new List<Element>();
            foreach (var entity in Environment.Entities)
                elements.AddRange(entity.Elements);
            colliders = new List<ICollider>(elements
                .Where(e => e is ICollider)
                .Select(e => e as ICollider));
        }
        public override void Update(double time)
        {
            ICollider colliding = null;

            IsCollidingX = false;
            IsCollidingY = false;
            IsCollidingZ = false;

            do
            {
                lastPos = currPos;
                currPos = transform.Position;

                var move = currPos - lastPos;
                var othermove = move;
                var ray = new Ray(lastPos, Vector3.Normalize(move));

                //if (move.X < GAP && move.X > 0)
                //    move.X = GAP;
                //if (move.X > -GAP && move.X < 0)
                //    move.X = -GAP;

                //if (move.Y < GAP && move.Y > 0)
                //    move.Y = GAP;
                //if (move.Y > -GAP && move.Y < 0)
                //    move.Y = -GAP;

                //if (move.Z < GAP && move.Z > 0)
                //    move.Z = GAP;
                //if (move.Z > -GAP && move.Z < 0)
                //    move.Z = -GAP;

                colliding = colliders.FirstOrDefault(c =>
                {
                    var distance = c.Intersects(ray);
                    return distance.HasValue && move.LengthSquared > (distance.Value * distance.Value);
                });

                if (colliding != null)
                {
                    // Rays in alle Richtungen initializieren
                    var xyz = new
                    {
                        XP = new Ray(lastPos, Vector3.UnitX),
                        YP = new Ray(lastPos, Vector3.UnitY),
                        ZP = new Ray(lastPos, Vector3.UnitZ),
                        XM = new Ray(lastPos, -Vector3.UnitX),
                        YM = new Ray(lastPos, -Vector3.UnitY),
                        ZM = new Ray(lastPos, -Vector3.UnitZ)
                    };

                    // Distances festlegen, falls es welche gibt
                    var distance = new
                    {
                        X = colliding.Intersects(move.X > 0 ? xyz.XP : xyz.XM),
                        Y = colliding.Intersects(move.Y > 0 ? xyz.YP : xyz.YM),
                        Z = colliding.Intersects(move.Z > 0 ? xyz.ZP : xyz.ZM)
                    };

                    // Collisionen prüfen
                    var isColliding = new
                    {
                        X = distance.X.HasValue && Math.Abs(move.X) > distance.X.Value,
                        Y = distance.Y.HasValue && Math.Abs(move.Y) > distance.Y.Value,
                        Z = distance.Z.HasValue && Math.Abs(move.Z) > distance.Z.Value
                    };

					IsCollidingX = isColliding.X;
					IsCollidingY = isColliding.Y;
					IsCollidingZ = isColliding.Z;

                    float reduction;

                    //Collision verhindern
                    if (isColliding.X)
                    {
                        reduction = distance.X.Value / Math.Abs(move.X);
                        move.X *= reduction - GAP;

                        //if (move.X < GAP && move.X > 0)
                        //    move.X -= GAP;
                        //else if (move.X > -GAP && move.X < 0)
                        //    move.X += GAP;
                    }
                    else if (isColliding.Y)
                    {
                        reduction = distance.Y.Value / Math.Abs(move.Y);
                        move.Y *= reduction - GAP;

                        //if (move.Y < GAP && move.Y > 0)
                        //    move.Y -= GAP;
                        //else if (move.Y > -GAP && move.Y < 0)
                        //    move.Y += GAP;
                    }
                    else if (isColliding.Z)
                    {
                        reduction = distance.Z.Value / Math.Abs(move.Z);
                        move.Z *= reduction - GAP;
                    }

                    transform.Position = currPos = lastPos + move;
                }
            } while (colliding != null);
        }

        #region old code
        //if (false)//colliding != null)
        //{
        //    XRAY.Position = lastPos;
        //    YRAY.Position = lastPos;
        //    ZRAY.Position = lastPos;
        //    XRAM.Position = lastPos;
        //    YRAM.Position = lastPos;
        //    ZRAM.Position = lastPos;

        //    var Xdistance = colliding.Intersects(move.X > 0 ? XRAY : XRAM);
        //    var Ydistance = colliding.Intersects(move.Y > 0 ? YRAY : YRAM);
        //    var Zdistance = colliding.Intersects(move.Z > 0 ? ZRAY : ZRAM);

        //    var Xcolliding = Xdistance.HasValue && Math.Abs(move.X) > Xdistance.Value;
        //    var Ycolliding = Ydistance.HasValue && Math.Abs(move.Y) > Ydistance.Value;
        //    var Zcolliding = Zdistance.HasValue && Math.Abs(move.Z) > Zdistance.Value;

        //    float reduction;

        //    if (Xcolliding)
        //    {
        //        reduction = Xdistance.Value / Math.Abs(move.X);
        //        move.X *= reduction - GAP;
        //    }
        //    else if (Ycolliding)
        //    {
        //        reduction = Ydistance.Value / Math.Abs(move.Y);
        //        move.Y *= reduction - GAP;
        //    }
        //    else if (Zcolliding)
        //    {
        //        reduction = Zdistance.Value / Math.Abs(move.Z);
        //        move.Z *= reduction - GAP;
        //    }

        //    transform.Position = currPos = lastPos + move;
        //    XRAY.Position = Vector3.Zero;
        //    YRAY.Position = Vector3.Zero;
        //    ZRAY.Position = Vector3.Zero;
        //}

        //if (colliding != null)
        //{
        //    var distance = colliding.Intersects(ray).Value;
        //    var reduction = distance / move.Length();
        //    move *= reduction - (reduction / 10);
        //    transform.Position = currPos = lastPos + move;
        //}
        #endregion
    }
}