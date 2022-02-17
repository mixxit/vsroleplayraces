using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using vsroleplayraces.src.Foundation.Extensions;

namespace vsroleplayraces.src
{
    public class GuiDialogCreateCharacterExtendedRoleplay : GuiDialog
    {
        bool didSelect = false;
        protected IInventory characterInv;
        protected ElementBounds insetSlotBounds;
        int selectedRaceIndex = 0;
        int selectedTrait1Index = 0;
        int selectedTrait2Index = 0;
        int selectedIdealIndex = 0;
        int selectedFlawIndex = 0;
        int selectedBondIndex = 0;

        Dictionary<EnumCharacterDressType, int> DressPositionByTressType = new Dictionary<EnumCharacterDressType, int>();
        Dictionary<EnumCharacterDressType, ItemStack[]> DressesByDressType = new Dictionary<EnumCharacterDressType, ItemStack[]>();

        CharacterSystem modSys;
        int currentClassIndex = 0;

        int curTab = 0;
        int rows = 7;

        float charZoom = 1f;
        bool charNaked = true;

        protected int dlgHeight = 433 + 50;
        Dictionary<string, RaceDefaultAppearance> races;
        List<vsroleplayraces.src.Trait> traits;
        List<vsroleplayraces.src.Flaw> flaws;
        List<vsroleplayraces.src.Bond> bonds;
        List<vsroleplayraces.src.Ideal> ideals;

        public GuiDialogCreateCharacterExtendedRoleplay(ICoreClientAPI capi, CharacterSystem modSys) : base(capi)
        {
            this.modSys = modSys;
            this.races = capi.ModLoader.GetModSystem<VSRoleplayRacesMod>().GetRaces();
            this.traits = capi.ModLoader.GetModSystem<VSRoleplayRacesMod>().GetTraits();
            this.flaws = capi.ModLoader.GetModSystem<VSRoleplayRacesMod>().GetFlaws();
            this.bonds = capi.ModLoader.GetModSystem<VSRoleplayRacesMod>().GetBonds();
            this.ideals = capi.ModLoader.GetModSystem<VSRoleplayRacesMod>().GetIdeals();
        }

        public void LoadDefaultRace()
        {
            onToggleRace(JsonConvert.SerializeObject(this.races.Values.ToArray()[selectedRaceIndex]), true);
        }

        protected void ComposeGuis()
        {
            double pad = GuiElementItemSlotGridBase.unscaledSlotPadding;
            double slotsize = GuiElementPassiveItemSlot.unscaledSlotSize;

            characterInv = capi.World.Player.InventoryManager.GetOwnInventory(GlobalConstants.characterInvClassName);

            ElementBounds tabBounds = ElementBounds.Fixed(0, -25, 450, 25);

            double ypos = 20 + pad;



            ElementBounds bgBounds = ElementBounds.FixedSize(717, dlgHeight).WithFixedPadding(GuiStyle.ElementToDialogPadding);

            ElementBounds dialogBounds = ElementBounds.FixedSize(757, dlgHeight + 40).WithAlignment(EnumDialogArea.CenterMiddle)
                .WithFixedAlignmentOffset(GuiStyle.DialogToScreenPadding, 0);


            GuiTab[] tabs = new GuiTab[] {
                new GuiTab() { Name = "Race", DataInt = 0 },
                new GuiTab() { Name = "Class", DataInt = 1 },
              //  new GuiTab() { Name = "Outfit", DataInt = 2 }
            };

            Composers["createcharacter"] =
                capi.Gui
                .CreateCompo("createcharacter", dialogBounds)
                .AddShadedDialogBG(bgBounds, true)
                .AddDialogTitleBar(curTab == 0 ? Lang.Get("Race") : (curTab == 1 ? Lang.Get("Class") : Lang.Get("Outfit")), OnTitleBarClose)
                .AddHorizontalTabs(tabs, tabBounds, onTabClicked, CairoFont.WhiteSmallText().WithWeight(Cairo.FontWeight.Bold), CairoFont.WhiteSmallText().WithWeight(Cairo.FontWeight.Bold), "tabs")
                .BeginChildElements(bgBounds)
            ;

            capi.World.Player.Entity.hideClothing = false;

            // RACE
            if (curTab == 0)
            {
                var skinMod = capi.World.Player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();

                capi.World.Player.Entity.hideClothing = charNaked;
                var essr = capi.World.Player.Entity.Properties.Client.Renderer as EntitySkinnableShapeRenderer;
                essr.TesselateShape();

                CairoFont smallfont = CairoFont.WhiteSmallText();
                var tExt = smallfont.GetTextExtents(Lang.Get("Show dressed"));
                int colorIconSize = 22;

                ElementBounds leftColBounds = ElementBounds.Fixed(0, ypos, 204, dlgHeight - 59).FixedGrow(2 * pad, 2 * pad);

                insetSlotBounds = ElementBounds.Fixed(0, ypos + 2, 265, leftColBounds.fixedHeight - 2 * pad + 10).FixedRightOf(leftColBounds, 10);
                ElementBounds rightColBounds = ElementBounds.Fixed(0, ypos, 54, dlgHeight - 59).FixedGrow(2 * pad, 2 * pad).FixedRightOf(insetSlotBounds, 10);
                ElementBounds toggleButtonBounds = ElementBounds.Fixed(
                        (int)insetSlotBounds.fixedX + insetSlotBounds.fixedWidth / 2 - tExt.Width / RuntimeEnv.GUIScale / 2 - 12,
                        0,
                        tExt.Width / RuntimeEnv.GUIScale,
                        tExt.Height / RuntimeEnv.GUIScale
                    )
                    .FixedUnder(insetSlotBounds, 4).WithAlignment(EnumDialogArea.LeftFixed).WithFixedPadding(12, 6)
                ;

                double leftX = 0;
                ElementBounds prevbounds = null;
                ElementBounds bounds = ElementBounds.Fixed(leftX, (prevbounds == null || prevbounds.fixedY == 0) ? -10 : prevbounds.fixedY + 2, colorIconSize, colorIconSize);
                Composers["createcharacter"].AddRichtext("Race", CairoFont.WhiteSmallText(), bounds = bounds.BelowCopy(0, 10).WithFixedSize(210, 18));
                Composers["createcharacter"].AddDropDown(this.races.Values.Select(e => JsonConvert.SerializeObject(e)).ToArray(), this.races.Keys.ToArray(), selectedRaceIndex, (code, selected) => onToggleRace(code, selected), bounds = bounds.BelowCopy(0, 10).WithFixedSize(200, 22));
                prevbounds = bounds.FlatCopy();

                foreach (var skinpart in skinMod.AvailableSkinParts)
                {
                    bounds = ElementBounds.Fixed(leftX, (prevbounds == null || prevbounds.fixedY == 0) ? -10 : prevbounds.fixedY + 2, colorIconSize, colorIconSize);

                    string code = skinpart.Code;

                    AppliedSkinnablePartVariant appliedVar = skinMod.AppliedSkinParts.FirstOrDefault(sp => sp.PartCode == code);

                    if (skinpart.Type == EnumSkinnableType.Texture && !skinpart.UseDropDown)
                    {
                        // We want race to control body colour so do not show baseskin or haircolor
                        if (!skinpart.Code.Equals("baseskin") && !skinpart.Code.Equals("haircolor"))
                        {
                            int selectedIndex = 0;
                            int[] colors = new int[skinpart.Variants.Length];

                            for (int i = 0; i < skinpart.Variants.Length; i++)
                            {
                                colors[i] = skinpart.Variants[i].Color;

                                if (appliedVar?.Code == skinpart.Variants[i].Code) selectedIndex = i;
                            }

                            Composers["createcharacter"].AddRichtext(Lang.Get("skinpart-" + code), CairoFont.WhiteSmallText(), bounds = bounds.BelowCopy(0, 10).WithFixedSize(210, 22));
                            Composers["createcharacter"].AddColorListPicker(colors, (index) => onToggleSkinPartColor(code, index), bounds = bounds.BelowCopy(0, 0).WithFixedSize(colorIconSize, colorIconSize), 180, "picker-" + code);

                            for (int i = 0; i < colors.Length; i++)
                            {
                                var picker = Composers["createcharacter"].GetColorListPicker("picker-" + code + "-" + i);
                                picker.ShowToolTip = true;
                                picker.TooltipText = Lang.Get("color-" + skinpart.Variants[i].Code);

                                //Console.WriteLine("\"" + Lang.Get("color-" + skinpart.Variants[i].Code) + "\": \""+ skinpart.Variants[i].Code + "\"");
                            }

                            Composers["createcharacter"].ColorListPickerSetValue("picker-" + code, selectedIndex);
                        }
                    }
                    else
                    {
                        if (!skinpart.Code.Equals("voicetype") && !skinpart.Code.Equals("voicepitch"))
                        {

                            int selectedIndex = 0;

                            string[] names = new string[skinpart.Variants.Length];
                            string[] values = new string[skinpart.Variants.Length];

                            for (int i = 0; i < skinpart.Variants.Length; i++)
                            {
                                names[i] = Lang.Get("skinpart-" + code + "-" + skinpart.Variants[i].Code);
                                values[i] = skinpart.Variants[i].Code;

                                //Console.WriteLine("\"" + names[i] + "\": \"" + skinpart.Variants[i].Code + "\",");

                                if (appliedVar?.Code == values[i]) selectedIndex = i;
                            }


                            Composers["createcharacter"].AddRichtext(Lang.Get("skinpart-" + code), CairoFont.WhiteSmallText(), bounds = bounds.BelowCopy(0, 10).WithFixedSize(210, 22));

                            string tooltip = Lang.GetIfExists("skinpartdesc-" + code);
                            if (tooltip != null)
                            {
                                Composers["createcharacter"].AddHoverText(tooltip, CairoFont.WhiteSmallText(), 300, bounds = bounds.FlatCopy());
                            }

                            Composers["createcharacter"].AddDropDown(values, names, selectedIndex, (variantcode, selected) => onToggleSkinPartColor(code, variantcode), bounds = bounds.BelowCopy(0, 0).WithFixedSize(200, 25), "dropdown-" + code);
                        }
                    }

                    prevbounds = bounds.FlatCopy();

                    // Eye color is last on left side
                    if (skinpart.Code.Equals("mustache"))
                    {
                        leftX = insetSlotBounds.fixedX + insetSlotBounds.fixedWidth + 22;
                        prevbounds.fixedY = 0;
                    }
                }

                Composers["createcharacter"].AddRichtext("Ideal", CairoFont.WhiteSmallText(), bounds = bounds.BelowCopy(0, 10).WithFixedSize(210, 18));
                Composers["createcharacter"].AddDropDown(this.ideals.Select(e => e.id.ToString()).ToArray(), this.ideals.Select(e => e.description).ToArray(), selectedIdealIndex, (code, selected) => onTogglePersonality("ideal", code, selected), bounds = bounds.BelowCopy(0, 10).WithFixedSize(200, 22));
                prevbounds = bounds.FlatCopy();

                Composers["createcharacter"].AddRichtext("Flaw", CairoFont.WhiteSmallText(), bounds = bounds.BelowCopy(0, 10).WithFixedSize(210, 18));
                Composers["createcharacter"].AddDropDown(this.flaws.Select(e => e.id.ToString()).ToArray(), this.flaws.Select(e => e.description).ToArray(), selectedFlawIndex, (code, selected) => onTogglePersonality("flaw", code, selected), bounds = bounds.BelowCopy(0, 10).WithFixedSize(200, 22));
                prevbounds = bounds.FlatCopy();

                Composers["createcharacter"].AddRichtext("Bond", CairoFont.WhiteSmallText(), bounds = bounds.BelowCopy(0, 10).WithFixedSize(210, 18));
                Composers["createcharacter"].AddDropDown(this.bonds.Select(e => e.id.ToString()).ToArray(), this.bonds.Select(e => e.description).ToArray(), selectedBondIndex, (code, selected) => onTogglePersonality("bond", code, selected), bounds = bounds.BelowCopy(0, 10).WithFixedSize(200, 22));
                prevbounds = bounds.FlatCopy();

                Composers["createcharacter"].AddRichtext("Traits", CairoFont.WhiteSmallText(), bounds = bounds.BelowCopy(0, 10).WithFixedSize(210, 18));
                Composers["createcharacter"].AddDropDown(this.traits.Select(e => e.id.ToString()).ToArray(), this.traits.Select(e => e.description).ToArray(), selectedTrait1Index, (code, selected) => onTogglePersonality("trait1",code, selected), bounds = bounds.BelowCopy(0, 10).WithFixedSize(200, 22));
                Composers["createcharacter"].AddDropDown(this.traits.Select(e => e.id.ToString()).ToArray(), this.traits.Select(e => e.description).ToArray(), selectedTrait2Index, (code, selected) => onTogglePersonality("trait2", code, selected), bounds = bounds.BelowCopy(0, 10).WithFixedSize(200, 22));
                prevbounds = bounds.FlatCopy();

                Composers["createcharacter"]
                    .AddInset(insetSlotBounds, 2)
                    .AddToggleButton(Lang.Get("Show dressed"), smallfont, OnToggleDressOnOff, toggleButtonBounds, "showdressedtoggle")
                    .AddSmallButton(Lang.Get("Confirm Race"), OnNext, ElementBounds.Fixed(0, dlgHeight - 30).WithAlignment(EnumDialogArea.RightFixed).WithFixedPadding(12, 6), EnumButtonStyle.Normal, EnumTextOrientation.Center)
                ;

                Composers["createcharacter"].GetToggleButton("showdressedtoggle").SetValue(!charNaked);
            }

            // CLASS
            if (curTab == 1)
            {
                var essr = capi.World.Player.Entity.Properties.Client.Renderer as EntitySkinnableShapeRenderer;
                essr.TesselateShape();

                ypos -= 10;

                ElementBounds leftColBounds = ElementBounds.Fixed(0, ypos, 0, dlgHeight - 47).FixedGrow(2 * pad, 2 * pad);
                insetSlotBounds = ElementBounds.Fixed(0, ypos + 25, 190, leftColBounds.fixedHeight - 2 * pad + 10).FixedRightOf(leftColBounds, 10);

                //ElementBounds leftSlotBounds = ElementStdBounds.SlotGrid(EnumDialogArea.None, 0, ypos, 0, rows).FixedGrow(2 * pad, 2 * pad);
                //insetSlotBounds = ElementBounds.Fixed(0, ypos + 25, 190, leftSlotBounds.fixedHeight - 2 * pad + 8 + 25).FixedRightOf(leftSlotBounds, 10);

                ElementBounds rightSlotBounds = ElementStdBounds.SlotGrid(EnumDialogArea.None, 0, ypos, 1, rows).FixedGrow(2 * pad, 2 * pad).FixedRightOf(insetSlotBounds, 10);

                ElementBounds prevButtonBounds = ElementBounds.Fixed(0, ypos + 25, 35, slotsize - 4).WithFixedPadding(2).FixedRightOf(insetSlotBounds, 20);
                ElementBounds centerTextBounds = ElementBounds.Fixed(0, ypos + 25, 200, slotsize - 4 - 8).FixedRightOf(prevButtonBounds, 20);

                ElementBounds charclasssInset = centerTextBounds.ForkBoundingParent(4, 4, 4, 4);

                ElementBounds nextButtonBounds = ElementBounds.Fixed(0, ypos + 25, 35, slotsize - 4).WithFixedPadding(2).FixedRightOf(charclasssInset, 20);

                CairoFont font = CairoFont.WhiteMediumText();
                centerTextBounds.fixedY += (centerTextBounds.fixedHeight - font.GetFontExtents().Height / RuntimeEnv.GUIScale) / 2;

                ElementBounds charTextBounds = ElementBounds.Fixed(0, 0, 480, 100).FixedUnder(prevButtonBounds, 20).FixedRightOf(insetSlotBounds, 20);

                Composers["createcharacter"]
                    .AddInset(insetSlotBounds, 2)

                    .AddIconButton("left", (on) => changeClass(-1), prevButtonBounds.FlatCopy())
                    .AddInset(charclasssInset, 2)
                    .AddDynamicText("Commoner", font, EnumTextOrientation.Center, centerTextBounds, "className")
                    .AddIconButton("right", (on) => changeClass(1), nextButtonBounds.FlatCopy())

                    .AddRichtext("", CairoFont.WhiteDetailText(), charTextBounds, "characterDesc")
                    .AddSmallButton(Lang.Get("Confirm Class"), OnConfirm, ElementBounds.Fixed(0, dlgHeight - 30).WithAlignment(EnumDialogArea.RightFixed).WithFixedPadding(12, 6), EnumButtonStyle.Normal, EnumTextOrientation.Center)
                ;

                changeClass(0);
            }


            var tabElem = Composers["createcharacter"].GetHorizontalTabs("tabs");
            tabElem.unscaledTabSpacing = 20;
            tabElem.unscaledTabPadding = 10;
            tabElem.activeElement = curTab;

            Composers["createcharacter"].Compose();


            /*if (curTab == 2)
            {
                Composers["createcharacter"].GetSlotGrid("leftSlots").CanClickSlot = (slotid) => { return false; };
                //Composers["createcharacter"].GetSlotGrid("rightSlots").CanClickSlot = (slotid) => { return false; };
            }*/
        }

        private void OnToggleDressOnOff(bool on)
        {
            charNaked = !on;
            capi.World.Player.Entity.hideClothing = charNaked;
            var essr = capi.World.Player.Entity.Properties.Client.Renderer as EntitySkinnableShapeRenderer;
            essr.TesselateShape();
        }

        private void onToggleSkinPartColor(string partCode, string variantCode)
        {
            var skinMod = capi.World.Player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();
            skinMod.selectSkinPart(partCode, variantCode);
        }

        private void onTogglePersonality(string type, string idasstring, bool selected)
        {
            int value = Convert.ToInt32(idasstring);

            if (type.Equals("bond"))
            {
                for (int i = 0; i < this.bonds.Count; i++)
                {
                    if (this.bonds.ToArray()[i].id != value)
                        continue;
                    selectedBondIndex = i;
                }
            }
            if (type.Equals("trait1"))
            {
                for (int i = 0; i < this.traits.Count; i++)
                {
                    if (this.traits.ToArray()[i].id != value)
                        continue;
                    selectedTrait1Index = i;
                }
            }
            if (type.Equals("trait2"))
            {
                for (int i = 0; i < this.traits.Count; i++)
                {
                    if (this.traits.ToArray()[i].id != value)
                        continue;
                    selectedTrait2Index = i;
                }
            }
            if (type.Equals("ideal"))
            {
                for (int i = 0; i < this.ideals.Count; i++)
                {
                    if (this.ideals.ToArray()[i].id != value)
                        continue;
                    selectedIdealIndex = i;
                }
            }
            if (type.Equals("flaw"))
            {
                for (int i = 0; i < this.flaws.Count; i++)
                {
                    if (this.flaws.ToArray()[i].id != value)
                        continue;
                    selectedFlawIndex = i;
                }
            }
        }

        private void onToggleRace(string raceDefaultAppearanceAsJson, bool selected)
        {
            var raceDefaultAppearance = JsonConvert.DeserializeObject<RaceDefaultAppearance>(raceDefaultAppearanceAsJson);
            for (int i = 0; i < this.races.Values.Count; i++)
            {
                if (!this.races.Values.ToArray()[i].raceCode.Equals(raceDefaultAppearance.raceCode))
                    continue;
                selectedRaceIndex = i;
            }

            onToggleSkinPartColor("baseskin", raceDefaultAppearance.bodyCode);
            onToggleSkinPartColor("hairbase", raceDefaultAppearance.hairBase);
            onToggleSkinPartColor("haircolor", raceDefaultAppearance.hairColor);
            onToggleSkinPartColor("mustache", raceDefaultAppearance.mustache);
            onToggleSkinPartColor("beard", raceDefaultAppearance.beard);
            onToggleSkinPartColor("hairextra", raceDefaultAppearance.hairExtra);
            ComposeGuis();
        }

        private void onToggleSkinPartColor(string partCode, int index)
        {
            var skinMod = capi.World.Player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();

            string variantCode = skinMod.AvailableSkinPartsByCode[partCode].Variants[index].Code;

            skinMod.selectSkinPart(partCode, variantCode);
        }

        private bool OnNext()
        {
            curTab = 1;
            ComposeGuis();
            return true;
        }

        private void onTabClicked(int tabid)
        {
            curTab = tabid;
            ComposeGuis();
        }

        public override void OnGuiOpened()
        {
            string charclass = capi.World.Player.Entity.WatchedAttributes.GetString("characterClass");
            if (charclass != null)
            {
                modSys.setCharacterClass(capi.World.Player.Entity, charclass, true);
            }
            else
            {
                modSys.setCharacterClass(capi.World.Player.Entity, modSys.characterClasses[0].Code, true);
            }

            ComposeGuis();
            LoadDefaultRace();
            var essr = capi.World.Player.Entity.Properties.Client.Renderer as EntitySkinnableShapeRenderer;
            essr.TesselateShape();

            if (capi.World.Player.WorldData.CurrentGameMode == EnumGameMode.Guest || capi.World.Player.WorldData.CurrentGameMode == EnumGameMode.Survival)
            {
                if (characterInv != null) characterInv.Open(capi.World.Player);
            }
        }


        public override void OnGuiClosed()
        {
            if (characterInv != null)
            {
                characterInv.Close(capi.World.Player);
                Composers["createcharacter"].GetSlotGrid("leftSlots")?.OnGuiClosed(capi);
                Composers["createcharacter"].GetSlotGrid("rightSlots")?.OnGuiClosed(capi);
            }

            CharacterClass chclass = modSys.characterClasses[currentClassIndex];

            modSys.ClientRaceSelectionDone(this.capi, GetCurrentRaceName(), GetCurrentIdealId(), GetCurrentTrait1Id(), GetCurrentTrait2Id(), GetCurrentFlawId(), GetCurrentBondId());
            modSys.ClientSelectionDone(characterInv, chclass.Code, didSelect);

            capi.World.Player.Entity.hideClothing = false;
            var essr = capi.World.Player.Entity.Properties.Client.Renderer as EntitySkinnableShapeRenderer;
            essr.TesselateShape();
        }

        private string GetCurrentRaceName()
        {
            return this.races.Values.ToArray()[selectedRaceIndex].raceCode;
        }

        private int GetCurrentIdealId()
        {
            return this.ideals.ToArray()[selectedIdealIndex].id;
        }

        private int GetCurrentTrait1Id()
        {
            return this.traits.ToArray()[selectedTrait1Index].id;
        }

        private int GetCurrentTrait2Id()
        {
            return this.traits.ToArray()[selectedTrait2Index].id;
        }

        private int GetCurrentFlawId()
        {
            return this.traits.ToArray()[selectedFlawIndex].id;
        }

        private int GetCurrentBondId()
        {
            return this.traits.ToArray()[selectedBondIndex].id;
        }

        private bool OnConfirm()
        {
            didSelect = true;
            TryClose();
            return true;
        }

        protected virtual void OnTitleBarClose()
        {
            TryClose();
        }
        protected void SendInvPacket(object packet)
        {
            capi.Network.SendPacketClient(packet);
        }

        void changeClass(int dir)
        {
            currentClassIndex = GameMath.Mod(currentClassIndex + dir, modSys.characterClasses.Count);

            CharacterClass chclass = modSys.characterClasses[currentClassIndex];
            Composers["createcharacter"].GetDynamicText("className").SetNewText(Lang.Get("characterclass-" + chclass.Code));

            StringBuilder fulldesc = new StringBuilder();
            StringBuilder attributes = new StringBuilder();

            fulldesc.AppendLine(Lang.Get("characterdesc-" + chclass.Code));
            fulldesc.AppendLine();
            fulldesc.AppendLine(Lang.Get("traits-title"));

            var chartraits = chclass.Traits.Select(code => modSys.TraitsByCode[code]).OrderBy(trait => (int)trait.Type);

            foreach (var trait in chartraits)
            {
                attributes.Clear();
                foreach (var val in trait.Attributes)
                {
                    if (attributes.Length > 0) attributes.Append(", ");
                    attributes.Append(Lang.Get(string.Format(GlobalConstants.DefaultCultureInfo, "charattribute-{0}-{1}", val.Key, val.Value)));
                }

                if (attributes.Length > 0)
                {
                    fulldesc.AppendLine(Lang.Get("traitwithattributes", Lang.Get("trait-" + trait.Code), attributes));
                }
                else
                {
                    string desc = Lang.GetIfExists("traitdesc-" + trait.Code);
                    if (desc != null)
                    {
                        fulldesc.AppendLine(Lang.Get("traitwithattributes", Lang.Get("trait-" + trait.Code), desc));
                    }
                    else
                    {
                        fulldesc.AppendLine(Lang.Get("trait-" + trait.Code));
                    }


                }
            }

            if (chclass.Traits.Length == 0)
            {
                fulldesc.AppendLine("No positive or negative traits");
            }

            Composers["createcharacter"].GetRichtext("characterDesc").SetNewText(fulldesc.ToString(), CairoFont.WhiteDetailText());

            modSys.setCharacterClass(capi.World.Player.Entity, chclass.Code, true);

            var essr = capi.World.Player.Entity.Properties.Client.Renderer as EntitySkinnableShapeRenderer;
            essr.TesselateShape();
        }


        public void PrepAndOpen()
        {
            GatherDresses(EnumCharacterDressType.Foot);
            GatherDresses(EnumCharacterDressType.Hand);
            GatherDresses(EnumCharacterDressType.Shoulder);
            GatherDresses(EnumCharacterDressType.UpperBody);
            GatherDresses(EnumCharacterDressType.LowerBody);
            TryOpen();
        }

        private void GatherDresses(EnumCharacterDressType type)
        {
            List<ItemStack> dresses = new List<ItemStack>();
            dresses.Add(null);

            string stringtype = type.ToString().ToLowerInvariant();

            IList<Item> items = capi.World.Items;

            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];
                if (item == null || item.Code == null || item.Attributes == null) continue;

                string clothcat = item.Attributes["clothescategory"]?.AsString();
                bool allow = item.Attributes["inCharacterCreationDialog"]?.AsBool() == true;

                if (allow && clothcat?.ToLowerInvariant() == stringtype)
                {
                    dresses.Add(new ItemStack(item));
                }
            }

            DressesByDressType[type] = dresses.ToArray();
            DressPositionByTressType[type] = 0;
        }




        public override bool CaptureAllInputs()
        {
            return IsOpened();
        }


        public override string ToggleKeyCombinationCode
        {
            get { return null; }
        }



        public override void OnMouseWheel(MouseWheelEventArgs args)
        {
            base.OnMouseWheel(args);

            if (insetSlotBounds.PointInside(capi.Input.MouseX, capi.Input.MouseY) && curTab == 0)
            {
                charZoom = GameMath.Clamp(charZoom + args.deltaPrecise / 5f, 0.5f, 1f);
            }
        }

        public override bool PrefersUngrabbedMouse => true;


        #region Character render 
        protected float yaw = -GameMath.PIHALF + 0.3f;
        protected bool rotateCharacter;
        public override void OnMouseDown(MouseEvent args)
        {
            base.OnMouseDown(args);

            rotateCharacter = insetSlotBounds.PointInside(args.X, args.Y);
        }

        public override void OnMouseUp(MouseEvent args)
        {
            base.OnMouseUp(args);

            rotateCharacter = false;
        }

        public override void OnMouseMove(MouseEvent args)
        {
            base.OnMouseMove(args);

            if (rotateCharacter) yaw -= args.DeltaX / 100f;
        }


        Vec4f lighPos = new Vec4f(-1, -1, 0, 0).NormalizeXYZ();
        Matrixf mat = new Matrixf();

        public override void OnRenderGUI(float deltaTime)
        {
            base.OnRenderGUI(deltaTime);

            capi.Render.GlPushMatrix();

            if (focused) { capi.Render.GlTranslate(0, 0, 150); }

            capi.Render.GlRotate(-14, 1, 0, 0);

            mat.Identity();
            mat.RotateXDeg(-14);
            Vec4f lightRot = mat.TransformVector(lighPos);
            double pad = GuiElement.scaled(GuiElementItemSlotGridBase.unscaledSlotPadding);

            capi.Render.CurrentActiveShader.Uniform("lightPosition", new Vec3f(lightRot.X, lightRot.Y, lightRot.Z));

            capi.Render.PushScissor(insetSlotBounds);

            if (curTab == 0)
            {
                capi.Render.RenderEntityToGui(
                    deltaTime,
                    capi.World.Player.Entity,
                    insetSlotBounds.renderX + pad - GuiElement.scaled(195) * charZoom + GuiElement.scaled(115 * (1 - charZoom)),
                    insetSlotBounds.renderY + pad + GuiElement.scaled(10 * (1 - charZoom)),
                    (float)GuiElement.scaled(230),
                    yaw,
                    (float)GuiElement.scaled(330 * charZoom),
                    ColorUtil.WhiteArgb);
            }
            else
            {
                capi.Render.RenderEntityToGui(
                    deltaTime,
                    capi.World.Player.Entity,
                    insetSlotBounds.renderX + pad - GuiElement.scaled(95),
                    insetSlotBounds.renderY + pad - GuiElement.scaled(0),
                    (float)GuiElement.scaled(230),
                    yaw,
                    (float)GuiElement.scaled(180),
                    ColorUtil.WhiteArgb);
            }

            capi.Render.PopScissor();

            capi.Render.CurrentActiveShader.Uniform("lightPosition", new Vec3f(1, -1, 0).Normalize());

            capi.Render.GlPopMatrix();
        }
        #endregion


        public override float ZSize
        {
            get { return (float)GuiElement.scaled(280); }
        }
    }
}