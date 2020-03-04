using System.Linq;
using UnityEngine;

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable MemberCanBePrivate.Global

namespace ModuleLightBulb
{
    public class ModuleLightBulb : ModuleLight
    {
        [KSPField(guiActive = true,
            guiActiveEditor = true,
            guiName = "B.U.L.B. Control",
            isPersistant = true,
            groupName = "Bulb",
            groupDisplayName = "B.U.L.B. Control",
            groupStartCollapsed = true)]
        public static string bulbStatus = "";

        [KSPField(guiActive = true,
            guiActiveEditor = true,
            guiName = "#autoLOC_6001403",
            isPersistant = true,
            groupName = "Bulb",
           // groupDisplayName = "B.U.L.B. Control",
            groupStartCollapsed = true)]
        [UI_FloatRange(maxValue = 1, minValue = 0, stepIncrement = 0.01f)]
        private float _red = 0.72f;
        protected float red { get => _red; set => _red = value; }

        [KSPField(guiActive = true,
            guiActiveEditor = true,
            guiName = "#autoLOC_6001402",
            isPersistant = true,
            groupName = "Bulb")]
        [UI_FloatRange(maxValue = 1, minValue = 0, stepIncrement = 0.01f)]
        private float _green = 0.35f;
        protected float green { get => _green; set => _green = value; }

        [KSPField(guiActive = true,
            guiActiveEditor = true,
            guiName = "#autoLOC_6001404",
            isPersistant = true,
            groupName = "Bulb")]
        [UI_FloatRange(maxValue = 1, minValue = 0, stepIncrement = 0.01f)]
        private float _blue = 0.0f;
        protected float blue { get => _blue; set => _blue = value; }

        [KSPField(guiActive = true,
            guiName = "Emissive Multiplier",
            isPersistant = true,
            groupName = "Bulb")]
        [UI_FloatRange(maxValue = 3, minValue = 0, stepIncrement = 0.01f)]
        protected float emissiveMultipier = 1.73f;

        [KSPField(guiActive = true,
            guiName = "Emissive Brightness",
            isPersistant = true,
            groupName = "Bulb")]
        [UI_FloatRange(stepIncrement = 0.01f, maxValue = 1f, minValue = 0f)]
        public float lensBrightness = 0.0f;


        public override void OnInitialize()
        {
            base.OnInitialize();
            lightR = red;
            lightG = green;
            lightB = blue;
            UpdateLightTextureColor();
        }

        public override void OnAwake()
        {
            base.OnAwake();
            Fields["lensBrightness"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["emissiveMultipier"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["green"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["blue"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["red"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["lightG"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["lightB"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["lightR"].OnValueModified += (x => UpdateLightTextureColor());

        }

        public virtual void UpdateLightTextureColor()
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                red = lightR;
                green = lightG;
                blue = lightB;
            }
            part.FindModelComponents<Light>().ToList().ForEach(r =>
            {
                r.color = new Color(red, green, blue, 1);
            });
            part.FindModelComponents<Renderer>()
                .Where(r => r.material.HasProperty("_EmissiveColor"))
                .ToList()
                .ForEach(r => r.material.SetColor("_EmissiveColor", GetEmissiveTextureColor()));
            if (HighLogic.LoadedSceneIsEditor)
            {
                emissiveMultipier += 0.01f;
                emissiveMultipier -= 0.01f;
            }
        }

        protected virtual Color GetEmissiveTextureColor()
        {
            return new Color((red * (1.0f - lensBrightness) + lensBrightness) * emissiveMultipier,
                             (green * (1.0f - lensBrightness) + lensBrightness) * emissiveMultipier,
                             (blue * (1.0f - lensBrightness) + lensBrightness) * emissiveMultipier);
        }
        public override void OnStart(StartState state)
        {
            
           // Fields["lightR"].group.name = "Bulb";
            Fields["lightR"].guiActiveEditor = false;
           // Fields["lightG"].group.name = "Bulb";
            Fields["lightG"].guiActiveEditor = false;
           // Fields["lightB"].group.name = "Bulb";
            Fields["lightB"].guiActiveEditor = false;
        }
    }
}