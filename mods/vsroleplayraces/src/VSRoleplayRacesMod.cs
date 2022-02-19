using Vintagestory.API.Common;
using System;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using Vintagestory.API.Client;
using Foundation.Extensions;
using vsroleplayraces.src.Foundation.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace vsroleplayraces.src
{
    public class VSRoleplayRacesMod : ModSystem
    {
        Dictionary<string, RaceDefaultSettings> races = new Dictionary<string, RaceDefaultSettings>();
        List<vsroleplayraces.src.Trait> traits = new List<vsroleplayraces.src.Trait>();
        List<vsroleplayraces.src.Flaw> flaws = new List<vsroleplayraces.src.Flaw>();
        List<vsroleplayraces.src.Bond> bonds = new List<vsroleplayraces.src.Bond>();
        List<vsroleplayraces.src.Ideal> ideals = new List<vsroleplayraces.src.Ideal>();

        private ICoreAPI api;
        public override void StartPre(ICoreAPI api)
        {
            VSRoleplayRacesModConfigFile.Current = api.LoadOrCreateConfig<VSRoleplayRacesModConfigFile>(typeof(VSRoleplayRacesMod).Name + ".json");
            base.StartPre(api);
        }
        public override void Start(ICoreAPI api)
        {
            SetupRaces();
            SetIdeals();
            SetBonds();
            SetFlaws();
            SetTraits();

            this.api = api;
            base.Start(api);
            api.Network
                .RegisterChannel("raceselection")
                .RegisterMessageType<RaceSelectionPacket>();
        }

        private void SetupRaces()
        {
            this.races.Clear();
            this.races.Add("human", new RaceDefaultSettings() { raceCode = "human", bodyCode = "skin11", hairBase = "shortspiky", defaultHairColor = -14270355, allowedHairColors = new List<int>() { -2699319, -6382693, -13093323, -15592942, -9268607, -10914730, -8025523, -5531019, -6781614, -8829884, -5858886, -10928289, -12703691, -5390384, -6507849, -11562833, -13599294, -13481590, -15256462, -14270355, -15456433, -16247499 }.ToArray(), mustache = "none", beard = "none", hairExtra = "none", strength = 85, stamina = 85, agility = 80, dexterity = 75, intelligence = 75, wisdom = 75, charisma = 75, alignment = AlignmentType.Neutral, description = "Humans make the most of being ingenuitive and somewhat average at everything. Sure - they may not be the strongest or most intelligent but their high fertility rate and well-roundedness allows them to compete with other races by merely applying more bodies to a problem" });
            this.races.Add("darkelf", new RaceDefaultSettings() { raceCode = "darkelf", bodyCode = "skin4", hairBase = "combed", defaultHairColor = -2699319, allowedHairColors = new List<int>() { -2699319, -13093323, -8025523, -12703691, -5390384, -6507849 }.ToArray(), mustache = "none", beard = "none", hairExtra = "none", strength = 70, stamina = 70, agility = 90, dexterity = 75, intelligence = 109, wisdom = 83, charisma = 65, alignment = AlignmentType.Evil, description = "For every Elf that stands under the blazing sun there lays beneath a Dark Elf. This subterranean matriarchal culture is fueled equally by one part malice and one part deceit. Every layer of this society looks up with envy, downwards with disgust and outwards with total xenophobia" });
            this.races.Add("orc", new RaceDefaultSettings() { raceCode = "orc", bodyCode = "skin20", hairBase = "iroquois", defaultHairColor = -15256462, allowedHairColors = new List<int>() { -2699319, -6382693, -13093323, -15592942, -9268607, -10914730, -8025523, -5531019, -6781614, -8829884, -5858886, -10928289, -12703691, -5390384, -6507849, -11562833, -13599294, -13481590, -15256462, -14270355, -15456433, -16247499 }.ToArray(), mustache = "none", beard = "none", hairExtra = "none", strength = 140, stamina = 127, agility = 70, dexterity = 80, intelligence = 60, wisdom = 67, charisma = 37, alignment = AlignmentType.Evil, description = "Despite their extremely low intelligence and charisma, the Orcs have found they can solve most problems with a hammer. Few can rival their strength or stamina and, in numbers, the many tribes and orc warbands spell doom for any that stands between them and their objectives" });
            this.races.Add("highelf", new RaceDefaultSettings() { raceCode = "highelf", bodyCode = "skin9", hairBase = "longwithstrands", defaultHairColor = -11562833, allowedHairColors = new List<int>() { -2699319, -6382693, -5858886, -5390384, -6507849, -11562833, -15456433 }.ToArray(), mustache = "none", beard = "none", hairExtra = "none", strength = 65, stamina = 70, agility = 85, dexterity = 70, intelligence = 92, wisdom = 100, charisma = 90, alignment = AlignmentType.Good, description = "Ancient, graceful and completely in control of their mind and body. The beauty and wisdom of an Elf is a sight to be seen. Their long lives, strategic thinking and unmatched mastery over magic has allowed Elf societies to sit at the peak of world politics" });
            this.races.Add("vampire", new RaceDefaultSettings() { raceCode = "vampire", bodyCode = "skin15", hairBase = "longflowing", defaultHairColor = -13093323, allowedHairColors = new List<int>() { -2699319, -6382693, -13093323, -15592942, -8829884, -10928289, -12703691, -5390384, -6507849  }.ToArray(), mustache = "none", beard = "none", hairExtra = "none", strength = 70, stamina = 70, agility = 90, dexterity = 75, intelligence = 109, wisdom = 83, charisma = 65, alignment = AlignmentType.Evil, description = "It is said that when the Sun sleeps the world is governed by the many Great Vampire Houses. This ancient race follows only one edict, blood over everything. Royalty, cruelty and dominion are their domain and all that fall under their pale gaze will, inevitably, submit" });
        }

        private void SetIdeals()
        {
            this.ideals.Clear();
            this.ideals.Add(new Ideal(1, "Faith", "I trust that my deity will guide my actions. I have faith that if I work hard, things will go well.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(2, "Tradition", "The ancient traditions of worship and sacrifice must be preserved and upheld.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(3, "Charity", "I always try to help those in need, no matter what the personal cost.", AlignmentType.Good));
            this.ideals.Add(new Ideal(4, "Change", "We must help bring about the changes the gods are constantly working in the world.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(5, "Power", "I hope to one day rise to the top of my faith's religious hierarchy.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(6, "Aspiration", "I seek to prove my self worthy of my god's favor by matching my actions against his or her teachings.", AlignmentType.Any));
            this.ideals.Add(new Ideal(7, "Independence", "I am a free spirit--no one tells me what to do.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(8, "Fairness", "I never target people who can't afford to lose a few coins.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(9, "Charity", "I distribute money I acquire to the people who really need it.", AlignmentType.Good));
            this.ideals.Add(new Ideal(10, "Creativity", "I never run the same con twice.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(11, "Friendship", "Material goods come and go. Bonds of friendship last forever.", AlignmentType.Good));
            this.ideals.Add(new Ideal(12, "Aspiration", "I'm determined to make something of myself.", AlignmentType.Any));
            this.ideals.Add(new Ideal(13, "Honor", "I don't steal from others in the trade.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(14, "Freedom", "Chains are meant to be broken, as are those who would forge them.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(15, "Charity", "I steal from the wealthy so that I can help people in need.", AlignmentType.Good));
            this.ideals.Add(new Ideal(16, "Greed", "I will do whatever it takes to become wealthy.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(17, "People", "I'm loyal to my friends, not to any ideals, and everyone else can take a trip down the Styx for all I care.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(18, "Redemption", "There's a spark of good in everyone.", AlignmentType.Good));
            this.ideals.Add(new Ideal(19, "Beauty", "When I perform, I make the world better than it was.", AlignmentType.Good));
            this.ideals.Add(new Ideal(20, "Tradition", "The stories, legends, and songs of the past must never be forgotten.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(21, "Creativity", "The world is in need of new ideas and bold action.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(22, "Greed", "I'm only in it for the money and fame.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(23, "People", "I like seeing the smiles on people's faces when I perform. That's all that matters.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(24, "Honesty", "Art should reflect the soul; it should come from within and reveal who we really are.", AlignmentType.Any));
            this.ideals.Add(new Ideal(25, "Respect", "People deserve to be treated with dignity and respect.", AlignmentType.Good));
            this.ideals.Add(new Ideal(26, "Fairness", "No one should get preferential treatment before the law, and no one is above the law.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(27, "Freedom", "Tyrants must not be allowed to oppress the people.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(28, "Might", "If I become strong, I can take what I want--what I deserve.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(29, "Sincerity", "There's no good pretending to be something I'm not.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(30, "Destiny", "Nothing and no one can steer me away from my higher calling.", AlignmentType.Any));
            this.ideals.Add(new Ideal(31, "Community", "It is the duty of all civilized people to strengthen the bonds of community and the security of civilization.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(32, "Generosity", "My talents were given to me so that I could use them to benefit the world.", AlignmentType.Good));
            this.ideals.Add(new Ideal(33, "Freedom", "Everyone should be free to pursue his or her livelihood.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(34, "Greed", "I'm only in it for the money.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(35, "People", "I'm committed to the people I care about, not to ideals.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(36, "Aspiration", "I work hard to be the best there is at my craft.", AlignmentType.Any));
            this.ideals.Add(new Ideal(37, "Greater Good", "My gifts are meant to be shared with all, not used for my own benefit.", AlignmentType.Good));
            this.ideals.Add(new Ideal(38, "Logic", "Emotions must not cloud our sense of what is right and true, or our logical thinking.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(39, "Free Thinking", "Inquiry and curiosity are the pillars of progress.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(40, "Power", "Solitude and contemplation are paths toward mystical or magical power.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(41, "Live and Let Live", "Meddling in the affairs of others only causes trouble.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(42, "Self-Knowledge", "If you know yourself, there're nothing left to know.", AlignmentType.Any));
            this.ideals.Add(new Ideal(43, "Respect", "Respect is due to me because of my position, but all people regardless of station deserve to be treated with dignity.", AlignmentType.Good));
            this.ideals.Add(new Ideal(44, "Responsibility", "It is my duty to respect the authority of those above me, just as those below me must respect mine.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(45, "Independence", "I must prove that I can handle myself without the coddling of my family.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(46, "Power", "If I can attain more power, no one will tell me what to do.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(47, "Family", "Blood runs thicker than water.", AlignmentType.Any));
            this.ideals.Add(new Ideal(48, "Noble Obligation", "It is my duty to protect and care for the people beneath me.", AlignmentType.Good));
            this.ideals.Add(new Ideal(49, "Change", "Life is like the seasons, in constant change, and we must change with it.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(50, "Greater Good", "It is each person's responsibility to make the most happiness for the whole tribe.", AlignmentType.Good));
            this.ideals.Add(new Ideal(51, "Honor", "If I dishonor myself, I dishonor my whole clan.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(52, "Might", "The strongest are meant to rule.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(53, "Nature", "The natural world is more important than all the constructs of civilization.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(54, "Glory", "I must earn glory in battle, for myself and my clan.", AlignmentType.Any));
            this.ideals.Add(new Ideal(55, "Knowledge", "The path to power and self-improvement is through knowledge.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(56, "Beauty", "What is beautiful points us beyond itself toward what is true.", AlignmentType.Good));
            this.ideals.Add(new Ideal(57, "Logic", "Emotions must not cloud our logical thinking.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(58, "No Limits", "Nothing should fetter the infinite possibility inherent in all existence.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(59, "Power", "Knowledge is the path to power and domination.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(60, "Self-improvement", "The goal of a life of study is the betterment of oneself.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(61, "Respect", "The thing that keeps a ship together is mutual respect between captain and crew.", AlignmentType.Good));
            this.ideals.Add(new Ideal(62, "Fairness", "We all do the work, so we all share in the rewards.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(63, "Freedom", "The sea is freedom--the freedom to go anywhere and do anything.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(64, "Master", "I'm a predator, and the other ships on the sea are my prey.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(65, "People", "I'm committed to my crewmates, not to ideals.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(66, "Aspiration", "Someday I'll own my own ship and chart my own destiny.", AlignmentType.Any));
            this.ideals.Add(new Ideal(67, "Greater Good", "Our lot is to lay down our lives in defense of others.", AlignmentType.Good));
            this.ideals.Add(new Ideal(68, "Responsibility", "I do what I must and obey just authority.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(69, "Independence", "When people follow orders blindly they embrace a kind of tyranny.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(70, "Might", "In life as in war, the stronger force wins.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(71, "Live and Let Live", "Ideals aren't worth killing for or going to war for.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(72, "Nation", "My city, nation, or people are all that matter.", AlignmentType.Any));
            this.ideals.Add(new Ideal(73, "Respect", "All people, rich or poor, deserve respect.", AlignmentType.Good));
            this.ideals.Add(new Ideal(74, "Community", "We have to take care of each other, because no one else is going to do it.", AlignmentType.Lawful));
            this.ideals.Add(new Ideal(75, "Change", "The low are lifted up, and the high and mighty are brought down. Change is the nature of things.", AlignmentType.Chaotic));
            this.ideals.Add(new Ideal(76, "Retribution", "The rich need to be shown what life and death are like in the gutters.", AlignmentType.Evil));
            this.ideals.Add(new Ideal(77, "People", "I help people who help me--that's what keeps us alive.", AlignmentType.Neutral));
            this.ideals.Add(new Ideal(78, "Aspiration", "I'm going to prove that I'm worthy of a better life.", AlignmentType.Any));
        }

        private void SetBonds()
        {
            this.bonds.Clear();
            this.bonds.Add(new Bond(1, "I would die to recover an ancient artifact of my faith that was lost long ago."));
            this.bonds.Add(new Bond(2, "I will someday get revenge on the corrupt temple hierarchy who branded me a heretic."));
            this.bonds.Add(new Bond(3, "I owe me life to the priest who took me in when my parents died."));
            this.bonds.Add(new Bond(4, "Everything I do is for the common people."));
            this.bonds.Add(new Bond(5, "I will do anything to protect the temple where I served."));
            this.bonds.Add(new Bond(6, "I seek to preserve a sacred text that my enemies consider heretical and seek to destroy."));
            this.bonds.Add(new Bond(7, "I fleeced the wrong person and must work to ensure that this individual never crosses paths with me or those I care about."));
            this.bonds.Add(new Bond(8, "I owe everything to my mentor--a horrible person who's probably rotting in jail somewhere."));
            this.bonds.Add(new Bond(9, "Somewhere out there I have a child who doesn't know me. I'm making the world better for him or her."));
            this.bonds.Add(new Bond(10, "I come from a noble family, and one day I'll reclaim my lands and title from those who stole them from me."));
            this.bonds.Add(new Bond(11, "A powerful person killed someone I love. Some day soon, I'll have my revenge."));
            this.bonds.Add(new Bond(12, "I swindled and ruined a person who didn't deserve it. I seek to atone for my misdeeds but might never be able to forgive myself."));
            this.bonds.Add(new Bond(13, "I'm trying to pay off an old debt I owe to a generous benefactor."));
            this.bonds.Add(new Bond(14, "My ill-gotten gains go to support my family."));
            this.bonds.Add(new Bond(15, "Something important was taken from me, and I aim to steal it back."));
            this.bonds.Add(new Bond(16, "I will become the greatest thief that ever lived."));
            this.bonds.Add(new Bond(17, "I'm guilty of a terrible crime. I hope I can redeem myself for it."));
            this.bonds.Add(new Bond(18, "Someone I loved died because of a mistake I made. That will never happen again."));
            this.bonds.Add(new Bond(19, "My instrument is my most treasured possession, and it reminds me of someone I love."));
            this.bonds.Add(new Bond(20, "Someone stole my precious instrument, and someday I'll get it back."));
            this.bonds.Add(new Bond(21, "I want to be famous, whatever it takes."));
            this.bonds.Add(new Bond(22, "I idolize a hero of the old tales and measure my deeds against that person's."));
            this.bonds.Add(new Bond(23, "I will do anything to prove myself superior to me hated rival."));
            this.bonds.Add(new Bond(24, "I would do anything for the other members of my old troupe."));
            this.bonds.Add(new Bond(25, "I have a family, but I have no idea where they are. One day, I hope to see them again."));
            this.bonds.Add(new Bond(26, "I worked the land, I love the land, and I will protect the land."));
            this.bonds.Add(new Bond(27, "A proud noble once gave me a horrible beating, and I will take my revenge on any bully I encounter."));
            this.bonds.Add(new Bond(28, "My tools are symbols of my past life, and I carry them so that I will never forget my roots."));
            this.bonds.Add(new Bond(29, "I protect those who cannot protect themselves."));
            this.bonds.Add(new Bond(30, "I wish my childhood sweetheart had come with me to pursue my destiny."));
            this.bonds.Add(new Bond(31, "The workshop where I learned my trade is the most important place in the world to me."));
            this.bonds.Add(new Bond(32, "I created a great work for someone, and then found them unworthy to receive it. I'm still looking for someone worthy."));
            this.bonds.Add(new Bond(33, "I owe my guild a great debt for forging me into the person I am today."));
            this.bonds.Add(new Bond(34, "I pursue wealth to secure someone's love."));
            this.bonds.Add(new Bond(35, "One day I will return to my guild and prove that I am the greatest artisan of them all."));
            this.bonds.Add(new Bond(36, "I will get revenge on the evil forces that destroyed my place of business and ruined my livelihood."));
            this.bonds.Add(new Bond(37, "Nothing is more important than the other members of my hermitage, order, or association."));
            this.bonds.Add(new Bond(38, "I entered seclusion to hide from the ones who might still be hunting me. I must someday confront them."));
            this.bonds.Add(new Bond(39, "I'm still seeking the enlightenment I pursued in my seclusion, and it still eludes me."));
            this.bonds.Add(new Bond(40, "I entered seclusion because I loved someone I could not have."));
            this.bonds.Add(new Bond(41, "Should my discovery come to light, it could bring ruin to the world."));
            this.bonds.Add(new Bond(42, "My isolation gave me great insight into a great evil that only I can destroy."));
            this.bonds.Add(new Bond(43, "I will face any challenge to win the approval of my family."));
            this.bonds.Add(new Bond(44, "My house's alliance with another noble family must be sustained at all costs."));
            this.bonds.Add(new Bond(45, "Nothing is more important that the other members of my family."));
            this.bonds.Add(new Bond(46, "I am in love with the heir of a family that my family despises."));
            this.bonds.Add(new Bond(47, "My loyalty to my sovereign is unwavering."));
            this.bonds.Add(new Bond(48, "The common folk must see me as a hero of the people."));
            this.bonds.Add(new Bond(49, "My family, clan, or tribe is the most important thing in my life, even when they are far from me."));
            this.bonds.Add(new Bond(50, "An injury to the unspoiled wilderness of my home is an injury to me."));
            this.bonds.Add(new Bond(51, "I will bring terrible wrath down on the evildoers who destroyed my homeland."));
            this.bonds.Add(new Bond(52, "I am the last of my tribe, and it is up to me to ensure their names enter legend."));
            this.bonds.Add(new Bond(53, "I suffer awful visions of a coming disaster and will do anything to prevent it."));
            this.bonds.Add(new Bond(54, "It is my duty to provide children to sustain my tribe."));
            this.bonds.Add(new Bond(55, "It is my duty to protect my students."));
            this.bonds.Add(new Bond(56, "I have an ancient text that holds terrible secrets that must not fall into the wrong hands."));
            this.bonds.Add(new Bond(57, "I work to preserve a library, university, scriptorium, or monastery."));
            this.bonds.Add(new Bond(58, "My life's work is a series of tomes related to a specific field of lore."));
            this.bonds.Add(new Bond(59, "I've been searching my whole life for the answer to a certain question."));
            this.bonds.Add(new Bond(60, "I sold my soul for knowledge. I hope to do great deeds and win it back."));
            this.bonds.Add(new Bond(61, "I'm loyal to my captain first, everything else second."));
            this.bonds.Add(new Bond(62, "The ship is most important--crewmates and captains come and go."));
            this.bonds.Add(new Bond(63, "I'll always remember my first ship."));
            this.bonds.Add(new Bond(64, "In a harbor town, I have a paramour whose eyes nearly stole me from the sea."));
            this.bonds.Add(new Bond(65, "I was cheated of my fair share of the profits, and I want to get my due."));
            this.bonds.Add(new Bond(66, "Ruthless pirates murdered my captain and crewmates, plundered our ship, and left me to die. Vengeance will be mine."));
            this.bonds.Add(new Bond(67, "I would lay down my life for the people I served with."));
            this.bonds.Add(new Bond(68, "Someone saved my life on the battlefield. To this day, I will never leave a friend behind."));
            this.bonds.Add(new Bond(69, "My honor is my life."));
            this.bonds.Add(new Bond(70, "I'll never forget the crushing defeat my company suffered or the enemies who dealt it."));
            this.bonds.Add(new Bond(71, "Those who fight beside me are those worth dying for."));
            this.bonds.Add(new Bond(72, "I fight for those who cannot fight for themselves."));
            this.bonds.Add(new Bond(73, "My town or city is my home, and I'll fight to defend it."));
            this.bonds.Add(new Bond(74, "I sponsor an orphanage to keep others from enduring what I was forced to endure."));
            this.bonds.Add(new Bond(75, "I owe my survival to another urchin who taught me to live on the streets."));
            this.bonds.Add(new Bond(76, "I owe a debt I can never repay to the person who took pity on me."));
            this.bonds.Add(new Bond(77, "I escaped my life of poverty by robbing an important person, and I'm wanted for it."));
            this.bonds.Add(new Bond(78, "No one else is going to have to endure the hardships I've been through."));
        }

        public void SetFlaws()
        {
            this.flaws.Clear();
            this.flaws.Add(new Flaw(1, "I judge others harshly, and myself even more severely."));
            this.flaws.Add(new Flaw(2, "I put too much trust in those who wield power within my temple's hierarchy."));
            this.flaws.Add(new Flaw(3, "My piety sometimes leads me to blindly trust those that profess faith in my god."));
            this.flaws.Add(new Flaw(4, "I am inflexible in my thinking."));
            this.flaws.Add(new Flaw(5, "I am suspicious of strangers and suspect the worst of them."));
            this.flaws.Add(new Flaw(6, "Once I pick a goal, I become obsessed with it to the detriment of everything else in my life."));
            this.flaws.Add(new Flaw(7, "I can't resist a pretty face."));
            this.flaws.Add(new Flaw(8, "I'm always in debt. I spend my ill-gotten gains on decadent luxuries faster than I bring them in."));
            this.flaws.Add(new Flaw(9, "I'm convinced that no one could ever fool me in the way I fool others."));
            this.flaws.Add(new Flaw(10, "I'm too greedy for my own good. I can't resist taking a risk if there's money involved."));
            this.flaws.Add(new Flaw(11, "I can't resist swindling people who are more powerful than me."));
            this.flaws.Add(new Flaw(12, "I hate to admit it and will hate myself for it, but I'll run and preserve my own hide if the going gets tough."));
            this.flaws.Add(new Flaw(13, "When I see something valuable, I can't think about anything but how to steal it."));
            this.flaws.Add(new Flaw(14, "When faced with a choice between money and my friends, I usually choose the money."));
            this.flaws.Add(new Flaw(15, "If there's a plan, I'll forget it. If I don't forget it, I'll ignore it."));
            this.flaws.Add(new Flaw(16, "I have a 'tell' that reveals when I'm lying."));
            this.flaws.Add(new Flaw(17, "I turn tail and run when things go bad."));
            this.flaws.Add(new Flaw(18, "An innocent person is in prison for a crime that I committed. I'm okay with that."));
            this.flaws.Add(new Flaw(19, "I'll do anything to win fame and renown."));
            this.flaws.Add(new Flaw(20, "I'm a sucker for a pretty face."));
            this.flaws.Add(new Flaw(21, "A scandal prevents me from ever going home again. That kind of trouble seems to follow me around."));
            this.flaws.Add(new Flaw(22, "I once satirized a noble who still wants my head. It was a mistake that I will likely repeat."));
            this.flaws.Add(new Flaw(23, "I have trouble keeping my true feelings hidden. My sharp tongue lands me in trouble."));
            this.flaws.Add(new Flaw(24, "Despite my best efforts, I am unreliable to my friends."));
            this.flaws.Add(new Flaw(25, "The tyrant who rules my land will stop at nothing to see me killed."));
            this.flaws.Add(new Flaw(26, "I'm convinced of the significance of my destiny, and blind to my shortcomings and the risk of failure."));
            this.flaws.Add(new Flaw(27, "The people who knew me when I was young know my shameful secret, so I can never go home again."));
            this.flaws.Add(new Flaw(28, "I have a weakness for the vices of the city, especially hard drink."));
            this.flaws.Add(new Flaw(29, "Secretly, I believe that things would be better if I were a tyrant lording over the land."));
            this.flaws.Add(new Flaw(30, "I have trouble trusting in my allies."));
            this.flaws.Add(new Flaw(31, "I'll do anything to get my hands on something rare or priceless."));
            this.flaws.Add(new Flaw(32, "I'm quick to assume that someone is trying to cheat me."));
            this.flaws.Add(new Flaw(33, "No one must ever learn that I once stole money from guild coffers."));
            this.flaws.Add(new Flaw(34, "I'm never satisfied with what I have--I always want more."));
            this.flaws.Add(new Flaw(35, "I would kill to acquire a noble title."));
            this.flaws.Add(new Flaw(36, "I'm horribly jealous of anyone who outshines my handiwork. Everywhere I go, I'm surrounded by rivals."));
            this.flaws.Add(new Flaw(37, "Now that I've returned to the world, I enjoy its delights a little too much."));
            this.flaws.Add(new Flaw(38, "I harbor dark bloodthirsty thoughts that my isolation failed to quell."));
            this.flaws.Add(new Flaw(39, "I am dogmatic in my thoughts and philosophy."));
            this.flaws.Add(new Flaw(40, "I let my need to win arguments overshadow friendships and harmony."));
            this.flaws.Add(new Flaw(41, "I'd risk too much to uncover a lost bit of knowledge."));
            this.flaws.Add(new Flaw(42, "I like keeping secrets and won't share them with anyone."));
            this.flaws.Add(new Flaw(43, "I secretly believe that everyone is beneath me."));
            this.flaws.Add(new Flaw(44, "I hide a truly scandalous secret that could ruin my family forever."));
            this.flaws.Add(new Flaw(45, "I too often hear veiled insults and threats in every word addressed to me, and I'm quick to anger."));
            this.flaws.Add(new Flaw(46, "I have an insatiable desire for carnal pleasures."));
            this.flaws.Add(new Flaw(47, "In fact, the world does revolve around me."));
            this.flaws.Add(new Flaw(48, "By my words and actions, I often bring shame to my family."));
            this.flaws.Add(new Flaw(49, "I am too enamored of ale, wine, and other intoxicants."));
            this.flaws.Add(new Flaw(50, "There's no room for caution in a life lived to the fullest."));
            this.flaws.Add(new Flaw(51, "I remember every insult I've received and nurse a silent resentment toward anyone who's ever wronged me."));
            this.flaws.Add(new Flaw(52, "I am slow to trust members of other races"));
            this.flaws.Add(new Flaw(53, "Violence is my answer to almost any challenge."));
            this.flaws.Add(new Flaw(54, "Don't expect me to save those who can't save themselves. It is nature's way that the strong thrive and the weak perish."));
            this.flaws.Add(new Flaw(55, "I am easily distracted by the promise of information."));
            this.flaws.Add(new Flaw(56, "Most people scream and run when they see a demon. I stop and take notes on its anatomy."));
            this.flaws.Add(new Flaw(57, "Unlocking an ancient mystery is worth the price of a civilization."));
            this.flaws.Add(new Flaw(58, "I overlook obvious solutions in favor of complicated ones."));
            this.flaws.Add(new Flaw(59, "I speak without really thinking through my words, invariably insulting others."));
            this.flaws.Add(new Flaw(60, "I can't keep a secret to save my life, or anyone else's."));
            this.flaws.Add(new Flaw(61, "I follow orders, even if I think they're wrong."));
            this.flaws.Add(new Flaw(62, "I'll say anything to avoid having to do extra work."));
            this.flaws.Add(new Flaw(63, "Once someone questions my courage, I never back down no matter how dangerous the situation."));
            this.flaws.Add(new Flaw(64, "Once I start drinking, it's hard for me to stop."));
            this.flaws.Add(new Flaw(65, "I can't help but pocket loose coins and other trinkets I come across."));
            this.flaws.Add(new Flaw(66, "My pride will probably lead to my destruction"));
            this.flaws.Add(new Flaw(67, "The monstrous enemy we faced in battle still leaves me quivering with fear."));
            this.flaws.Add(new Flaw(68, "I have little respect for anyone who is not a proven warrior."));
            this.flaws.Add(new Flaw(69, "I made a terrible mistake in battle that cost many lives--and I would do anything to keep that mistake secret."));
            this.flaws.Add(new Flaw(70, "My hatred of my enemies is blind and unreasoning."));
            this.flaws.Add(new Flaw(71, "I obey the law, even if the law causes misery."));
            this.flaws.Add(new Flaw(72, "I'd rather eat my armor than admit when I'm wrong."));
            this.flaws.Add(new Flaw(73, "If I'm outnumbered, I always run away from a fight."));
            this.flaws.Add(new Flaw(74, "Gold seems like a lot of money to me, and I'll do just about anything for more of it."));
            this.flaws.Add(new Flaw(75, "I will never fully trust anyone other than myself."));
            this.flaws.Add(new Flaw(76, "I'd rather kill someone in their sleep than fight fair."));
            this.flaws.Add(new Flaw(77, "It's not stealing if I need it more than someone else."));
            this.flaws.Add(new Flaw(78, "People who don't take care of themselves get what they deserve."));
        }

        public void SetTraits()
        {
            this.traits.Clear();
            this.traits.Add(new Trait(1, "I idolize a particular hero of my faith and constantly refer to that person's deeds and example."));
            this.traits.Add(new Trait(2, "I can find common ground between the fiercest enemies, empathizing with them and always working toward peace."));
            this.traits.Add(new Trait(3, "I see omens in every event and action. The gods try to speak to us, we just need to listen."));
            this.traits.Add(new Trait(4, "Nothing can shake my optimistic attitude."));
            this.traits.Add(new Trait(5, "I quote (or misquote) the sacred texts and proverbs in almost every situation."));
            this.traits.Add(new Trait(6, "I am tolerant (or intolerant) of other faiths and respect (or condemn) the worship of other gods."));
            this.traits.Add(new Trait(7, "I've enjoyed fine food, drink, and high society among my temple's elite. Rough living grates on me."));
            this.traits.Add(new Trait(8, "I've spent so long in the temple that I have little practical experience dealing with people in the outside world."));
            this.traits.Add(new Trait(9, "I fall in and out of love easily, and am always pursuing someone."));
            this.traits.Add(new Trait(10, "I have a joke for every occasion, especially occasions where humor is inappropriate."));
            this.traits.Add(new Trait(11, "Flattery is my preferred trick for getting what I want."));
            this.traits.Add(new Trait(12, "I'm a born gambler who can't resist taking a risk for a potential payoff."));
            this.traits.Add(new Trait(13, "I lie about almost everything, even when there's no good reason to."));
            this.traits.Add(new Trait(14, "Sarcasm and insults are my weapons of choice."));
            this.traits.Add(new Trait(15, "I keep multiple holy symbols on me and invoke whatever deity might come in useful at any given moment."));
            this.traits.Add(new Trait(16, "I pocket anything I see that might have some value."));
            this.traits.Add(new Trait(17, "I always have plan for what to do when things go wrong."));
            this.traits.Add(new Trait(18, "I am always calm, no matter what the situation. I never raise my voice or let my emotions control me."));
            this.traits.Add(new Trait(19, "The first thing I do in a new place is note the locations of everything valuable--or where such things could be hidden."));
            this.traits.Add(new Trait(20, "I would rather make a new friend than a new enemy."));
            this.traits.Add(new Trait(21, "I am incredibly slow to trust. Those who seem the fairest often have the most to hide."));
            this.traits.Add(new Trait(22, "I don't pay attention to the risks in a situation. Never tell me the odds."));
            this.traits.Add(new Trait(23, "The best way to get me to do something is to tell me I can't do it."));
            this.traits.Add(new Trait(24, "I blow up at the slightest insult."));
            this.traits.Add(new Trait(25, "I know a story relevant to almost every situation."));
            this.traits.Add(new Trait(26, "Whenever I come to a new place, I collect local rumors and spread gossip."));
            this.traits.Add(new Trait(27, "I'm a hopeless romantic, always searching for that 'special someone'."));
            this.traits.Add(new Trait(28, "Nobody stays angry at me or around me for long, since I can defuse any amount of tension."));
            this.traits.Add(new Trait(29, "I love a good insult, even one directed at me."));
            this.traits.Add(new Trait(30, "I get bitter if I'm not the center of attention."));
            this.traits.Add(new Trait(31, "I'll settle for nothing less than perfection."));
            this.traits.Add(new Trait(32, "I change my mood or my mind as quickly as I change key in a song."));
            this.traits.Add(new Trait(33, "I judge people by their actions, not their words."));
            this.traits.Add(new Trait(34, "If someone is in trouble, I'm always willing to lend help."));
            this.traits.Add(new Trait(35, "When I set my mind to something, I follow through no matter what gets in my way."));
            this.traits.Add(new Trait(36, "I have a strong sense of fair play and always try to find the most equitable solution to arguments."));
            this.traits.Add(new Trait(37, "I'm confident in my own abilities and do what I can to instill confidence in others."));
            this.traits.Add(new Trait(38, "Thinking is for other people. I prefer action."));
            this.traits.Add(new Trait(39, "I misuse long words in an attempt to sound smarter."));
            this.traits.Add(new Trait(40, "I get bored easily. When am I going to get on with my destiny."));
            this.traits.Add(new Trait(41, "I believe that everything worth doing is worth doing right. I can't help it--I'm a perfectionist."));
            this.traits.Add(new Trait(42, "I'm a snob who looks down on those who can't appreciate fine art."));
            this.traits.Add(new Trait(43, "I always want to know how things work and what makes people tick."));
            this.traits.Add(new Trait(44, "I'm full of witty aphorisms and have a proverb for every occasion."));
            this.traits.Add(new Trait(45, "I'm rude to people who lack my commitment to hard work and fair play."));
            this.traits.Add(new Trait(46, "I like to talk at length about my profession."));
            this.traits.Add(new Trait(47, "I don't part with my money easily and will haggle tirelessly to get the best deal possible."));
            this.traits.Add(new Trait(48, "I'm well known for my work, and I want to make sure everyone appreciates it. I'm always taken aback when people haven't heard of me."));
            this.traits.Add(new Trait(49, "I've been isolated for so long that I rarely speak, preferring gestures and the occasional grunt."));
            this.traits.Add(new Trait(50, "I am utterly serene, even in the face of disaster."));
            this.traits.Add(new Trait(51, "The leader of my community has something wise to say on every topic, and I am eager to share that wisdom."));
            this.traits.Add(new Trait(52, "I feel tremendous empathy for all who suffer."));
            this.traits.Add(new Trait(53, "I'm oblivious to etiquette and social expectations."));
            this.traits.Add(new Trait(54, "I connect everything that happens to me to a grand cosmic plan."));
            this.traits.Add(new Trait(55, "I often get lost in my own thoughts and contemplations, becoming oblivious to my surroundings."));
            this.traits.Add(new Trait(56, "I am working on a grand philosophical theory and love sharing my ideas."));
            this.traits.Add(new Trait(57, "My eloquent flattery makes everyone I talk to feel like the most wonderful and important person in the world."));
            this.traits.Add(new Trait(58, "The common folk love me for my kindness and generosity."));
            this.traits.Add(new Trait(59, "No one could doubt by looking at my regal bearing that I am a cut above the unwashed masses."));
            this.traits.Add(new Trait(60, "I take great pains to always look my best and follow the latest fashions."));
            this.traits.Add(new Trait(61, "I don't like to get my hands dirty, and I won't be caught dead in unsuitable accommodations."));
            this.traits.Add(new Trait(62, "Despite my birth, I do not place myself above other folk. We all have the same blood."));
            this.traits.Add(new Trait(63, "My favor, once lost, is lost forever."));
            this.traits.Add(new Trait(64, "If you do me an injury, I will crush you, ruin your name, and salt your fields."));
            this.traits.Add(new Trait(65, "I'm driven by a wanderlust that led me away from home."));
            this.traits.Add(new Trait(66, "I watch over my friends as if they were a litter of newborn pups."));
            this.traits.Add(new Trait(67, "I once ran twenty-five miles without stopping to warn my clan of an approaching orc horde. I'd do it again if I had to."));
            this.traits.Add(new Trait(68, "I have a lesson for every situation, drawn from observing nature."));
            this.traits.Add(new Trait(69, "I place no stock in wealthy or well-mannered folk. Money and manners won't save you from a hungry owlbear."));
            this.traits.Add(new Trait(70, "I'm always picking things up, absently fiddling with them, and sometimes accidentally breaking them."));
            this.traits.Add(new Trait(71, "I feel far more comfortable around animals than people."));
            this.traits.Add(new Trait(72, "I was, in fact, raised by wolves."));
            this.traits.Add(new Trait(73, "I use polysyllabic words to convey the impression of great erudition."));
            this.traits.Add(new Trait(74, "I've read every book in the world's greatest libraries--or like to boast that I have."));
            this.traits.Add(new Trait(75, "I'm used to helping out those who aren't as smart as I am, and I patiently explain anything and everything to others."));
            this.traits.Add(new Trait(76, "There's nothing I like more than a good mystery."));
            this.traits.Add(new Trait(77, "I'm willing to listen to every side of an argument before I make my own judgment."));
            this.traits.Add(new Trait(78, "I...speak...slowly...when talking...to idiots...which...almost...everyone...is...compared ...to me."));
            this.traits.Add(new Trait(79, "I am horribly, horribly awkward in social situations."));
            this.traits.Add(new Trait(80, "I'm convinced that people are always trying to steal my secrets."));
            this.traits.Add(new Trait(81, "My friends know they can rely on me, no matter what."));
            this.traits.Add(new Trait(82, "I work hard so that I can play hard when the work is done."));
            this.traits.Add(new Trait(83, "I enjoy sailing into new ports and making new friends over a flagon of ale."));
            this.traits.Add(new Trait(84, "I stretch the truth for the sake of a good story."));
            this.traits.Add(new Trait(85, "To me, a tavern brawl is a nice way to get to know a new city."));
            this.traits.Add(new Trait(86, "I never pass up a friendly wager."));
            this.traits.Add(new Trait(87, "My language is as foul as an otyugh nest."));
            this.traits.Add(new Trait(88, "I like a job well done, especially if I can convince someone else to do it."));
            this.traits.Add(new Trait(89, "I'm always polite and respectful."));
            this.traits.Add(new Trait(90, "I'm haunted by memories of war. I can't get the images of violence out of my mind."));
            this.traits.Add(new Trait(91, "I've lost too many friends, and I'm slow to make new ones."));
            this.traits.Add(new Trait(92, "I'm full of inspiring and cautionary tales from my military experience relevant to almost every combat situation."));
            this.traits.Add(new Trait(93, "I can stare down a hellhound without flinching."));
            this.traits.Add(new Trait(94, "I enjoy being strong and like breaking things."));
            this.traits.Add(new Trait(95, "I have a crude sense of humor."));
            this.traits.Add(new Trait(96, "I face problems head-on. A simple direct solution is the best path to success."));
            this.traits.Add(new Trait(97, "I hide scraps of food and trinkets away in my pockets."));
            this.traits.Add(new Trait(98, "I ask a lot of questions."));
            this.traits.Add(new Trait(99, "I like to squeeze into small places where no one else can get to me."));
            this.traits.Add(new Trait(100, "I sleep with my back to a wall or tree, with everything I own wrapped in a bundle in my arms."));
            this.traits.Add(new Trait(101, "I eat like a pig and have bad manners."));
            this.traits.Add(new Trait(102, "I think anyone who's nice to me is hiding evil intent."));
            this.traits.Add(new Trait(103, "I don't like to bathe."));
            this.traits.Add(new Trait(104, "I bluntly say what other people are hinting or hiding."));
        }

        internal Dictionary<string, RaceDefaultSettings> GetRaces()
        {
            return this.races;
        }

        internal List<Trait> GetTraits()
        {
            return this.traits;
        }

        internal List<Flaw> GetFlaws()
        {
            return this.flaws;
        }

        internal List<Bond> GetBonds()
        {
            return this.bonds;
        }

        internal List<Ideal> GetIdeals()
        {
            return this.ideals;
        }


        public override void StartClientSide(ICoreClientAPI api)
        {
            base.StartClientSide(api);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);
            api.Network.GetChannel("raceselection")
                .SetMessageHandler<RaceSelectionPacket>(onRaceSelection)
            ;
        }

        private void onRaceSelection(IServerPlayer fromPlayer, RaceSelectionPacket networkMessage)
        {
            var existingRace = fromPlayer.Entity.WatchedAttributes.GetString("racename");
            if (String.IsNullOrEmpty(existingRace) && this.races.Values.Select(e => e.raceCode).Contains(networkMessage.RaceName))
            {
                var race = this.races.Values.FirstOrDefault(e => e.raceCode.Equals(networkMessage.RaceName));
                fromPlayer.Entity.WatchedAttributes.SetString("racename", race.raceCode);
                fromPlayer.Entity.WatchedAttributes.SetInt("basestr", race.strength);
                fromPlayer.Entity.WatchedAttributes.SetInt("basesta", race.stamina);
                fromPlayer.Entity.WatchedAttributes.SetInt("baseagi", race.agility);
                fromPlayer.Entity.WatchedAttributes.SetInt("basedex", race.dexterity);
                fromPlayer.Entity.WatchedAttributes.SetInt("baseint", race.intelligence);
                fromPlayer.Entity.WatchedAttributes.SetInt("basewis", race.wisdom);
                fromPlayer.Entity.WatchedAttributes.SetInt("basecha", race.charisma);
            }

            var existingIdeal = fromPlayer.Entity.WatchedAttributes.GetInt("ideal");
            if (String.IsNullOrEmpty(existingRace) && this.ideals.Select(e => e.id).Contains(networkMessage.IdealId))
            {
                fromPlayer.Entity.WatchedAttributes.SetInt("ideal", networkMessage.IdealId);
            }

            var existingTrait1 = fromPlayer.Entity.WatchedAttributes.GetInt("trait1");
            if (String.IsNullOrEmpty(existingRace) && this.traits.Select(e => e.id).Contains(networkMessage.Trait1Id))
            {
                fromPlayer.Entity.WatchedAttributes.SetInt("trait1", networkMessage.Trait1Id);
            }

            var existingTrait2 = fromPlayer.Entity.WatchedAttributes.GetInt("trait2");
            if (String.IsNullOrEmpty(existingRace) && this.traits.Select(e => e.id).Contains(networkMessage.Trait2Id))
            {
                fromPlayer.Entity.WatchedAttributes.SetInt("trait2", networkMessage.Trait2Id);
            }

            var existingFlaw = fromPlayer.Entity.WatchedAttributes.GetInt("flaw");
            if (String.IsNullOrEmpty(existingRace) && this.flaws.Select(e => e.id).Contains(networkMessage.FlawId))
            {
                fromPlayer.Entity.WatchedAttributes.SetInt("flaw", networkMessage.FlawId);
            }

            var existingBond = fromPlayer.Entity.WatchedAttributes.GetInt("bond");
            if (String.IsNullOrEmpty(existingRace) && this.bonds.Select(e => e.id).Contains(networkMessage.BondId))
            {
                fromPlayer.Entity.WatchedAttributes.SetInt("bond", networkMessage.BondId);
            }


        }

        public override double ExecuteOrder()
        {
            /// Worldgen:
            /// - GenTerra: 0 
            /// - RockStrata: 0.1
            /// - Deposits: 0.2
            /// - Caves: 0.3
            /// - Blocklayers: 0.4
            /// Asset Loading
            /// - Json Overrides loader: 0.05
            /// - Load hardcoded mantle block: 0.1
            /// - Block and Item Loader: 0.2
            /// - Recipes (Smithing, Knapping, Clayforming, Grid recipes, Alloys) Loader: 1
            /// 
            return 1.1;
        }
    }

    public class VSRoleplayRacesModConfigFile
    {
        public static VSRoleplayRacesModConfigFile Current { get; set; }
    }
}
