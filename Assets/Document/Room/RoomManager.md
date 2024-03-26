加载房间场景的时候生成，
需要注意的是战斗场景的加载为LoadAddective
结算场景同样为LoadAddective

# 主要职责
1. 玩家进入的时候生成硬币，退出删除硬币
2. 角色的选择，多个人选择同一个角色，则按照顺序分配 Style0，Style1
1. 选择默认生命值
3. 所有玩家准备的时候，进入地图选择页面，由房主选择地图
1. 战斗的结算与房间的重置

## 硬币的生成

1. 硬币作为playerprefab，加入的时候生成到左上角硬币栏
2. IDragDrop... pos=Input.mousepos....
3. 进入角色块A，B,C,D，则执行角色选择，
4. 以外的区域，则返回原位置



RoomManager{
    List<Coin> coins;
    void Refresh(){
    //1. OnClientConnect和OnClientDisconnect的时候刷新界面
    //  遍历玩家列表,如果Coin.PlayerId不在玩家列表内,则删除Coin
    //  遍历Coins,如果玩家列表有一个playerId没有对应的coin则创建Coin
    }
}

Coin的创建,默认parent 为GO: Coins, 在移动的时候 parent为 GO: DragParent
Coin创建时候,如果未本地玩家,则可以拖动,并且名称改为You
Coin为networkTransform

coin.endDrag的时候,如果碰到FighterBlock,则设置position为endDrag的位置


## 角色选择

player player1;
player player2；
List<FighterAsset> classA;
List<FighterAsset> classB;

1. player1选择classA的index为0的
2. 如果player2选择classA则顺序遍历classA中selected不为true的；
3. player1选择classB，则顺序遍历classB中select为false的，同时标记lastClass.select为false
**需要的脚本**

一个玩家只可以选择一个GlortonFighterAsset
一个GlortonFighterAsset也只对应一个玩家


GlortonFigherAsset: SO
- GameObject 对应的玩家预制体
- GameObject 预览玩家预制体
- FighterSkinSheet 类型A,B,C,D
- bool selected

需要一个OnRoomInit的时候重置所有asset.selected，以及重置所有coin的位置

RoomPlayer: 
- GlortonFighterAsset 选择的角色

RoomManager{
    RoomPlayer player1;
        ....
    RoomPlayer player4;
}

RoomPlayer{
    NetworkVar<GlortonFighterAsset> selectedFighter;
}
GlortonFighterAsset通过id进行选择,根据下列代码的index读取对应的asset
GlortonFighterTable{
    List<GFAsset> fighters;
}


## 生命值选择

生命值在1~10之间浮动

**需要的脚本**

RoomManager{
    NetworkVar<short> playerHealth;
}