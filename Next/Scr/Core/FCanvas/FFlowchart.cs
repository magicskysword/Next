using System.Collections.Generic;
using System.Text;
using Fungus;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.Next.FCanvas;

public class FFlowchart : IModData
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<FBlock> Blocks = new List<FBlock>();

    public List<FVariable> Variables = new List<FVariable>();
    
    public int Version { get; set; } = 1;

    public void ReadFlowchart(Flowchart flowchart)
    {
        Id = flowchart.GetInstanceID();
        Name = flowchart.GetParentName();
            
        foreach (var block in flowchart.GetComponents<Block>())
        {
            var fBlock = new FBlock();
            fBlock.ReadBlock(block);
            Blocks.Add(fBlock);
        }

        foreach (var variable in flowchart.Variables)
        {
            var fVariable = new FVariable();
            fVariable.Key = variable.Key;
            fVariable.Type = variable.GetType().Name;
            switch (variable)
            {
                case StringVariable stringVariable:
                    fVariable.Value = stringVariable.Value;
                    break;
                case IntegerVariable integerVariable:
                    fVariable.Value = integerVariable.Value.ToString();
                    break;
                case FloatVariable floatVariable:
                    fVariable.Value = floatVariable.Value.ToString("F2");
                    break;
                case BooleanVariable booleanVariable:
                    fVariable.Value = booleanVariable.Value.ToString();
                    break;
                case GameObjectVariable gameObjectVariable:
                    fVariable.Value = gameObjectVariable.Value.name;
                    break;
                case TransformVariable transformVariable:
                    fVariable.Value = transformVariable.Value.name;
                    break;
                case Vector2Variable vector2Variable:
                    fVariable.Value = vector2Variable.Value.ToString();
                    break;
                case Vector3Variable vector3Variable:
                    fVariable.Value = vector3Variable.Value.ToString();
                    break;
                case ColorVariable colorVariable:
                    fVariable.Value = colorVariable.Value.ToString();
                    break;
                case MaterialVariable materialVariable:
                    fVariable.Value = materialVariable.Value.name;
                    break;
                case TextureVariable textureVariable:
                    fVariable.Value = textureVariable.Value.name;
                    break;
                case Rigidbody2DVariable rigidbody2DVariable:
                    fVariable.Value = rigidbody2DVariable.Value.name;
                    break;
                case ObjectVariable objectVariable:
                    fVariable.Value = objectVariable.Value.name;
                    break;
                case SpriteVariable spriteVariable:
                    fVariable.Value = spriteVariable.Value.name;
                    break;
                default:
                    fVariable.Value = "[Unknown]";
                    break;
            }
        }
    }
}