新的框架中,我们抛弃了coin的移动.


1. 游戏进入,SpawnPlayerServerRpc,同时检测是否生成

RoomManager{
    NetworkVar<RoomPlayerState>;
    RoomPlayerB

    OnPlayerJoin(){
        if(FindFirstASlot!=null){
        
        }else{
            disconnect;
        }
    }
    void FindFirstAvailibleSlot(){
        if(RoomPlayerA==null)
            return RoomPlayerA
    return null;
    }
}
RoomPlayerState{
    bool Connected,
    ulong ClientId,
    bool Selected,
    ...职业选择...
}

