随地图的加载而实例化
随地图的加载而卸载
# 主要职责

1. 根据RoomManager中每一个玩家选择的角色，生成MapPlayer
1. 游戏开始时倒计时
1. 玩家死亡播放死亡特效/音效
1. 玩家死亡1s后，玩家的重生

# 根据地图中的固定四个位置生成玩家
MapManager： Mono
- Transform[] spawnPos
- MapPlayer[] players
- Awake() 
    - 生成玩家，在指定位置，创建MapPlayer
    - 倒计时5秒
    - 倒计时结束，开启所有玩家的Input 

MapPlayer: 
- GlortonFighterAsset 选择的角色
- Health int 玩家生命值
- Dead bool 玩家是否已死亡（生命值为0，无法重生）
- rank int 玩家的排名， 第一个死亡的排名为0，第二个死亡的排名为2

# 特效播放
1. 玩家死亡/重生时
EventManager{Map{OnPlayerDeath,OnPlayerRespawn}}
由音效，特效系统监听此类事件

# 玩家重生
玩家死亡后，如果MapPlayer.health>0，则开始协程进行重生

MapManager： Mono
- Transform[] respawnPos 随机选择一个位置重生