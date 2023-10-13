enum AdjustProductInventoryType {
  Force = 1,
  IncreaseDecrease = 2
}

const AdjustProductInventoryTypeMap = new Map<AdjustProductInventoryType,string>([
  [AdjustProductInventoryType.Force,"強制設定至指定庫存"],
  [AdjustProductInventoryType.IncreaseDecrease,"增減庫存"]
])

export {AdjustProductInventoryType,AdjustProductInventoryTypeMap}