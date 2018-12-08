﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Tutorial.Core
{
    public class HierarchyInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private float _camAngle = 0;
        private TransformComponent _baseTransform;
        private TransformComponent _bodyTransform;
        private TransformComponent _upperArmTransform;
        private TransformComponent _lowerArmTransform;
        private TransformComponent _grabMiddleTransform;
        private TransformComponent _grabLeftTransform;
        private TransformComponent _grabRightTransform;
        
        
        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            _bodyTransform = new TransformComponent
            {
                Rotation = new float3 (0, 0.9f, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3 (0, 6, 0)   //hier wird die Position festgelegt, um 6 in Y-Richtung transaliert, da man die Position des Pivot Point festlegt, und der Arm 10 lang ist

            };

            _upperArmTransform = new TransformComponent
            {
                Rotation = new float3 (1.2f, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3 (2, 4, 0)  

            };

            _lowerArmTransform = new TransformComponent
            {
                Rotation = new float3 (-1.5f, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3 (-2, 8, 0)  

            };

            _grabMiddleTransform = new TransformComponent
            {
                Rotation = new float3 (0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3 (0, 5.25f, 0) 
            };

            _grabLeftTransform = new TransformComponent
            {
                Rotation = new float3 (0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3 (-0.75f, 6.0f, 0) 
            };

            _grabRightTransform = new TransformComponent
            {
                Rotation = new float3 (0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3 (0.75f, 6.0f, 0) 
            };


            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNodeContainer>
                {   // Grundplatte
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _baseTransform,

                            // Shader Effect
                            new ShaderEffectComponent
                            {
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0.7f, 0.7f, 0.7f), new float3(1, 1, 1), 5) 
                            },

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        }
                    },
                    //roter Arm
                    new SceneNodeContainer
                    {
                        Components = new List <SceneComponentContainer>
                        {
                            _bodyTransform,
                            new ShaderEffectComponent
                            {
                                 Effect = SimpleMeshes.MakeShaderEffect(new float3(1, 0, 0), new float3(1, 1, 1), 5) 
                            },
                        SimpleMeshes.CreateCuboid(new float3(2, 10, 2)) 
                        },
                     Children = new List <SceneNodeContainer>
                     {
                      //grüner Arm
                      new SceneNodeContainer
                      {
                        Components = new List <SceneComponentContainer>
                        {
                            _upperArmTransform,
                        },
                        Children = new List <SceneNodeContainer>
                        {
                            new SceneNodeContainer
                            {
                                Components = new List <SceneComponentContainer>
                                {
                                    new TransformComponent
                                    {
                                        Rotation = new float3(0, 0, 0),
                                        Scale = new float3(1, 1, 1),
                                        Translation = new float3(0, 4, 0)
                                    },
                                    new ShaderEffectComponent
                                    {
                                        Effect = SimpleMeshes.MakeShaderEffect(new float3(0, 1, 0), new float3(1, 1, 1), 5)
                                    },
                                    SimpleMeshes.CreateCuboid(new float3 (2, 10, 2))
                                }
                            },
                            //blauer Arm
                            new SceneNodeContainer
                            {
                                Components = new List<SceneComponentContainer>
                                {
                                    _lowerArmTransform,
                                },
                                Children = new List<SceneNodeContainer>
                                {
                                    new SceneNodeContainer
                                    {
                                        Components = new List<SceneComponentContainer>
                                        {
                                            new TransformComponent
                                            {
                                                Rotation = new float3(0, 0, 0),
                                                Scale = new float3(1, 1, 1),
                                                Translation = new float3(0, 4, 0)
                                            },
                                            new ShaderEffectComponent
                                            {
                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0, 0, 1), new float3(1, 1, 1), 5) 
                                            },
                                            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                        }
                                    },
                                    //middleGrap
                                    new SceneNodeContainer
                                    {
                                        Components = new List <SceneComponentContainer>
                                        {
                                            _grabMiddleTransform,
                                        },
                                        Children = new List <SceneNodeContainer>
                                        {
                                            new SceneNodeContainer
                                            {
                                                Components = new List <SceneComponentContainer>
                                                {
                                                    new TransformComponent
                                                    {
                                                        Rotation = new float3(0, 0, 0),
                                                        Scale = new float3(1, 1, 1),
                                                        Translation = new float3(0, 4, 0) 
                                                    },
                                                    new ShaderEffectComponent
                                                    {
                                                        Effect = SimpleMeshes.MakeShaderEffect(ColorUint.Tofloat3(ColorUint.Silver), new float3(1, 1, 1), 5)
                                                    },
                                                    SimpleMeshes.CreateCuboid(new float3(1, 0.5f, 0.5f))
                                                }
                                            }
                                        }
                                    },
                                    //leftGrap
                                    new SceneNodeContainer
                                    {
                                        Components = new List <SceneComponentContainer>
                                        {
                                            _grabLeftTransform,
                                        },
                                        Children = new List <SceneNodeContainer>
                                        {
                                            new SceneNodeContainer
                                            {
                                                Components = new List <SceneComponentContainer>
                                                {
                                                    new TransformComponent
                                                    {
                                                        Rotation = new float3(0, 0, 0),
                                                        Scale = new float3(1, 1, 1),
                                                        Translation = new float3(0, 4, 0) 
                                                    },
                                                    new ShaderEffectComponent
                                                    {
                                                        Effect = SimpleMeshes.MakeShaderEffect(ColorUint.Tofloat3(ColorUint.Silver), new float3(1, 1, 1), 5)
                                                    },
                                                    SimpleMeshes.CreateCuboid(new float3(0.5f, 2, 0.5f))
                                                }
                                            }
                                        }
                                    },
                                    //RightGrap
                                    new SceneNodeContainer
                                    {
                                        Components = new List <SceneComponentContainer>
                                        {
                                            _grabRightTransform,
                                        },
                                        Children = new List <SceneNodeContainer>
                                        {
                                            new SceneNodeContainer
                                            {
                                                Components = new List <SceneComponentContainer>
                                                {
                                                    new TransformComponent
                                                    {
                                                        Rotation = new float3(0, 0, 0),
                                                        Scale = new float3(1, 1, 1),
                                                        Translation = new float3(0, 4, 0) 
                                                    },
                                                    new ShaderEffectComponent
                                                    {
                                                        Effect = SimpleMeshes.MakeShaderEffect(ColorUint.Tofloat3(ColorUint.Silver), new float3(1, 1, 1), 5)
                                                    },
                                                    SimpleMeshes.CreateCuboid(new float3(0.5f, 2, 0.5f))
                                                }
                                            }
                                        }
                                    },
                                }
                            },
                           
                        }
                      },//grünerARm
                     } //Children roterArm
                    }
                }
            }; //SceneContainer
        } //SceneNodeContainer
                
            
        

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {   //Rotation rot
            float bodyRot = _bodyTransform.Rotation.y;
            bodyRot += 0.1f * Keyboard.LeftRightAxis;
            _bodyTransform.Rotation = new float3(0, bodyRot, 0);


            //rotation grün
            float armGreen = _upperArmTransform.Rotation.x;
            armGreen += 0.1f * Keyboard.UpDownAxis;
            _upperArmTransform.Rotation = new float3 (armGreen, 0, 0);
            

            //Rotation blau
            float armBlue = _lowerArmTransform.Rotation.x;
            armBlue += 0.1f * Keyboard.ADAxis;
            _lowerArmTransform.Rotation = new float3(armBlue, 0, 0);

            //Rotation Kamera
            
            if (Mouse.LeftButton == true)
            {
                _camAngle = _camAngle + Mouse.Velocity.x /600;
            } ;

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10, 50) * float4x4.CreateRotationY(_camAngle);

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}