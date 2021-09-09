from Query import Query

try:
    query = Query()

    
    query.setTable("테이블")
    query.SetGroup("그룹")
    query.AddColumn("이름","임나연")
    query.AddNVColumn("나이",26)
    query.AddCondition("나이 = '26'")
    query.AddOrder("DESC")
    query.SetLimit(1,2,3)

    print('strTable - ', query.strTable)
    print('strGroup - ', query.strGroup)
    print('strKeyColumn - ',query.strKeyColumn)
    print('aryColumn - ',query.aryColumn)
    print('aryColumnValue - ',query.aryColumnValue)
    print('aryColumnType - ',query.aryColumnType)
    print('aryCondition - ',query.aryCondition)
    print('aryOrder - ',query.aryOrder)
    print()

    InsertQuery = query.InsertQuery()
    UpdateQuery = query.UpdateQuery()
    DeleteQuery = query.DeleteQuery()
    
    TotalRowQuery = query.TotalRowQuery(True)
    SelectQueryNoLimit = query.SelectQueryNoLimit()


    print(InsertQuery)
    print(UpdateQuery)
    print(DeleteQuery)
    print()
    
    print(TotalRowQuery)
    print(SelectQueryNoLimit)
    print()

except Exception as e:
    print(e)




