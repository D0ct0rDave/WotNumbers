# uncompyle6 version 3.7.0
# Python bytecode 2.7 (62211)
# Decompiled from: Python 2.7.18 (v2.7.18:8d21aa21f2, Apr 20 2020, 13:19:08) [MSC v.1500 32 bit (Intel)]
# Embedded file name: scripts/common/battle_results/battle_results_constants.py
#from constants import ARENA_BONUS_TYPE
class ARENA_BONUS_TYPE:
    UNKNOWN = 0
    REGULAR = 1
    TRAINING = 2
    TOURNAMENT = 4
    CLAN = 5
    TUTORIAL = 6
    CYBERSPORT = 7
    EVENT_BATTLES = 9
    GLOBAL_MAP = 13
    TOURNAMENT_REGULAR = 14
    TOURNAMENT_CLAN = 15
    RATED_SANDBOX = 16
    SANDBOX = 17
    FALLOUT_CLASSIC = 18
    FALLOUT_MULTITEAM = 19
    SORTIE_2 = 20
    FORT_BATTLE_2 = 21
    RANKED = 22
    BOOTCAMP = 23
    EPIC_RANDOM = 24
    EPIC_RANDOM_TRAINING = 25
    EVENT_BATTLES_2 = 26
    EPIC_BATTLE = 27
    EPIC_BATTLE_TRAINING = 28
    BATTLE_ROYALE_SOLO = 29
    BATTLE_ROYALE_SQUAD = 30
    TOURNAMENT_EVENT = 31
    BOB = 32
    EVENT_RANDOM = 33
    BATTLE_ROYALE_TRN_SOLO = 34
    BATTLE_ROYALE_TRN_SQUAD = 35
    WEEKEND_BRAWL = 36
    MAPBOX = 37
    MAPS_TRAINING = 38
    RTS = 39
    RTS_1x1 = 40
    RTS_BOOTCAMP = 41
    RANGE = (
     UNKNOWN, REGULAR, TRAINING, TOURNAMENT, CLAN, TUTORIAL,
     CYBERSPORT, EVENT_BATTLES, EVENT_BATTLES_2, GLOBAL_MAP,
     TOURNAMENT_REGULAR, TOURNAMENT_CLAN, RATED_SANDBOX, SANDBOX,
     FALLOUT_CLASSIC, FALLOUT_MULTITEAM, BOOTCAMP, SORTIE_2, FORT_BATTLE_2, RANKED,
     EPIC_RANDOM, EPIC_RANDOM_TRAINING, EPIC_BATTLE, EPIC_BATTLE_TRAINING, TOURNAMENT_EVENT, EVENT_RANDOM,
     BATTLE_ROYALE_SOLO, BATTLE_ROYALE_SQUAD, BOB, BATTLE_ROYALE_TRN_SOLO, BATTLE_ROYALE_TRN_SQUAD,
     MAPBOX, WEEKEND_BRAWL, MAPS_TRAINING, RTS, RTS_1x1, RTS_BOOTCAMP)
    RANDOM_RANGE = (
     REGULAR, EPIC_RANDOM)
    SANDBOX_RANGE = (RATED_SANDBOX, SANDBOX)
    FALLOUT_RANGE = (FALLOUT_CLASSIC, FALLOUT_MULTITEAM)
    TOURNAMENT_RANGE = (TOURNAMENT, TOURNAMENT_REGULAR, TOURNAMENT_CLAN, TOURNAMENT_EVENT)
    BATTLE_ROYALE_RANGE = (BATTLE_ROYALE_SOLO, BATTLE_ROYALE_SQUAD, BATTLE_ROYALE_TRN_SOLO, BATTLE_ROYALE_TRN_SQUAD)
    BATTLE_ROYALE_REGULAR_RANGE = (BATTLE_ROYALE_SOLO, BATTLE_ROYALE_SQUAD)
    BATTLE_ROYALE_SQUAD_RANGE = (BATTLE_ROYALE_SQUAD, BATTLE_ROYALE_TRN_SQUAD)
    RTS_RANGE = (RTS, RTS_1x1, RTS_BOOTCAMP)
    RTS_BATTLES = (RTS, RTS_1x1)
    EXTERNAL_RANGE = (
     SORTIE_2, FORT_BATTLE_2, GLOBAL_MAP,
     TOURNAMENT, TOURNAMENT_CLAN, TOURNAMENT_REGULAR, TOURNAMENT_EVENT)

PATH_TO_CONFIG = {ARENA_BONUS_TYPE.REGULAR: 'random', 
   ARENA_BONUS_TYPE.EPIC_RANDOM: 'random', 
   ARENA_BONUS_TYPE.EPIC_RANDOM_TRAINING: 'random', 
   ARENA_BONUS_TYPE.RANKED: 'ranked', 
   ARENA_BONUS_TYPE.EPIC_BATTLE: 'frontline', 
   ARENA_BONUS_TYPE.EPIC_BATTLE_TRAINING: 'frontline', 
   ARENA_BONUS_TYPE.BATTLE_ROYALE_SOLO: 'battle_royale', 
   ARENA_BONUS_TYPE.BATTLE_ROYALE_SQUAD: 'battle_royale', 
   ARENA_BONUS_TYPE.BATTLE_ROYALE_TRN_SOLO: 'battle_royale', 
   ARENA_BONUS_TYPE.BATTLE_ROYALE_TRN_SQUAD: 'battle_royale', 
   ARENA_BONUS_TYPE.MAPBOX: 'random', 
   ARENA_BONUS_TYPE.MAPS_TRAINING: 'maps_training'}
POSSIBLE_TYPES = (
 int, float, str, bool, list, tuple, dict, set, None)

class BATTLE_RESULT_ENTRY_TYPE:
    COMMON = 1
    ACCOUNT_SELF = 2
    ACCOUNT_ALL = 3
    VEHICLE_SELF = 4
    VEHICLE_ALL = 5
    PLAYER_INFO = 6
    SERVER = 7
    ALL = (
     COMMON, ACCOUNT_SELF, ACCOUNT_ALL, VEHICLE_SELF, VEHICLE_ALL, PLAYER_INFO, SERVER)