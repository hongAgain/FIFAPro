module("Config", package.seeall)
require("Common/ktJson")


function OnInit()
    
end

function OnRelease()

end

local allConfigTable = {};

function ItemTable()
    return 'item';
end

function IconTable()
    return 'icon';
end

function ShopTable()
    return 'shop';
end

function LevelTable()
    return 'level';
end

function BaseTable()
    return 'base';
end

function HeroTable()
    return 'hero';
end

function HeroAPTable()
    return 'heroAP';
end

function HeroAdvTable()
    return 'heroAdv';
end

function HeroSlvTable()
    return 'heroSlv';
end

function HeroRatingTable()
    return 'heroRating';
end

function HeroAttrRatingTable()
    return 'heroAttriRating';
end

function HeroTeachTable()
    return 'heroTeach';
end

function GrowTable()
    return 'heroEvolve';
end

function HeroPotTable()
    return 'heroPot';
end

function HeroFragTable()
    return 'heroFrag';
end
function HeroPotStar()
    return 'potential_star'
end
function HeroPotAdv()
    return 'potential_advanced';
end
function HeroPotCost()
    return 'potential_cost';
end
function HeroPotTarget()
    return 'potential_target';
end
function HeroSkill()
    return 'skill';
end
function HeroSkillLvUp()
    return 'skill_lv_up';
end
function HeroSkillRefresh()
    return 'skill_refresh';
end
--function FormTable()
--    return 'formation';
--end

function SkillTipsTable()
    return "skill_tips"
end 

function ProTable()
    return 'profession';
end

function GroupClubTable()
    return 'groupClub';
end

function GroupCountryTable()
    return 'groupCountry';
end

function GroupBaseTable()
    return 'groupBase';
end

function ClubTable()
    return 'club';
end

function CountryTable()
   return 'country';
end

function FreePowerTable()
    return 'freePower';
end

function PayFirstTable()
    return 'payFirst';
end

function PayTotal()
    return 'payTotal';
end

function RaidStarAwrad()
    return 'raid_starAward';
end

function RaidClassTable()
    return 'raid_class';
end

function RaidLevelTable()
    return 'raid';
end

function RaidTeamTable()
    return 'raid_team';
end

--function RaidNpcTable()
--    return 'raid_npc';
--end

function RaidDSTable()
    return 'raidds';
end

function RaidDSVipTable()
    return 'raidds_vip';
end

function RaidDSNumsTable()
    return 'raidds_nums';
end

function RaidDSTeamTable()
    return 'raidds_team';
end

function RaidJYLevelTable()
    return 'raidjy';
end

function RaidJYTeamTable()
    return 'raidjy_team';
end

function RaidJYNpcTable()
    return 'raidjy_npc';
end

function AutoNameTeam()
    return 'autoNameTeam';
end

function AutoNamePlayer()
    return 'autoNamePlayer';
end

function RaidDFTable()
    return 'raiddf';
end

function RaidDFTeamTable()
    return 'raiddf_team';
end

function RaidDFNpcTable()
    return 'raiddf_npc';
end

function RaidDFVipTable()
    return 'raiddf_vip';
end

function RaidDFBuffTable()
    return 'raiddf_buff';
end

function RaidDFShop()
    return 'raiddf_shop';
end

function RobotTeamTable()
   return 'robotTeam';
end

function DailySignTable()
    return 'dailySign';
end

function DayTaskTable()
    return 'dayTask';
end

function DayTaskAwardTable()
    return 'dayTaskAward';
end

function DayTaskTypeTable()
    return 'dayTaskType';
end

function TaskTable()
    return 'task';
end

function TaskClassTable()
    return 'taskClass';
end

function TaskTypeTable()
    return 'taskType';
end

function LeagueTimeTable()
    return 'lgpvp';
end

function InitTeam()
   return 'initTeam';
end

function CupFinalAward()
    return 'cup_finalAward';
end

function CupGamTime()
    return 'cup_gamTime';
end

function CupMassAward()
    return 'cup_massAward';
end

function LadderLevel()
    return 'ladder_level';
end

function LadderEndLevel()
    return "ladder_endLevel"
end

function LadderShop()
    return 'ladder_shop';
end

function DismissShop()
    return 'sackplayer_shop';
end

function LadderRule()
    return 'ladder_rule';
end

function LeagueRule()
    return 'league_rule';
end

function DailyCupRule()
    return 'cup_rule';
end

function TimeRaidRule()
    return 'timeraid_rule';
end

function ModuleRules()
    return 'module_rules';
end

function VipBenefit()
    return 'vip_benefit'
end

function Invest()
    return 'invest'
end

function InvestRepay()
    return 'invest_repay'
end

function Login7Reward()
    return 'login7Reward'
end

function LoginAccReward()
    return 'loginAccReward'
end

function LevelGiftReward()
    return 'lvUpReward'
end

function LvRankReward()
    return 'LvRank'
end

function MidasType()
    return 'midas_Type'
end
function MidasBase()
    return 'midas_Base'
end
function MidasHand()
    return 'midas_Hand'
end

function Coach()
    return 'coach'
end

function CoachAdv()
    return 'coach_adv'
end

function CoachLv()
    return 'coach_lv'
end

function CoachPos()
    return 'coach_pos'
end

function CoachSkillSlot()
    return 'coach_skill_slot'
end

function CoachSkill()
    return 'coach_skill'
end

function CoachFameReward()
    return 'coach_fame_reward'
end

function CoachTalent()
    return 'coach_talent'
end

function CoachTalentPage(  )
    return 'coach_talent_page'
end

function PVELeague()
    return 'club_league'
end

function PVELeagueClub()
    return 'club_data'
end

function PVELeagueTeam()
    return 'club_team'
end

function PVELeagueReward()
    return 'club_league_reward'
end

function PVELeagueNPC()
    return 'club_npc'
end

function PVELeagueTeamGroup()
    return 'team_group'
end

function Carnival()
    return 'carnival'
end

function CarnivalType()
    return 'carnivalType'
end

function CarnivalPoint()
    return 'carnivalPoint'
end

function CarnivalShop()
    return 'carnivalShop'
end

activeTB        = 'active'
itemGroupTB     = 'itemGroup'
itemGroupInfoTB = "itemGroupInfo"
positionTB      = 'position'
initHeroTB      = 'initHero'
initAreaTB      = 'initArea'
heroAttTB       = 'heroAtt'
messageTB       = 'message'
broadcastTB     = 'broadcast'
gameTipsTB      = 'gameTips'
attTransTB      = 'attTrans'
tutorialTB      = 'Tutorial'

function OnConfigTemplateLoad(name, text)
	local content = json.decode(text);
	if allConfigTable[name] ~= nil then
		--print(name .. 'load repeat!');
	end;
    --print('load '..name);
	allConfigTable[name] = content;
	content = nil;
end

function GetTemplate(tbname)
	return allConfigTable[tbname];
end

function GetProperty(tbName, id, indexName)
    local tb = GetTemplate(tbName);
    if tb == nil then
        print("ERROR..table:"..tbName.." == nil!!!");
        return nil;
    end
--     print("ERROR..table:"..tbName);
    local conf = tb[id];
    if conf == nil then
        print("ERROR..conf:"..id.." == nil!!!"..tbName.."/"..indexName);
        return nil;
    end

    return conf[indexName];
end

function GetKeyTotalInfo( tbName, id )
    local tb = GetTemplate(tbName);
    if tb == nil then
        print("ERROR..table:"..tbName.." == nil!!!");
        return nil;
    end
    return tb[id]
end

function GetSamePropertyInfo( tbName, property, value )
    local tb = GetTemplate(tbName);
    if tb == nil then
        print("ERROR..table:"..tbName.." == nil!!!");
        return nil;
    end
    local ret = {}
    for k,v in pairs(tb) do
        if v[property] == value then
            ret[k] = v
        end
    end
    return ret
end
