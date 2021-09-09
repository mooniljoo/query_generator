from Database import Database

try:
    database = Database()

    query = "select * from TBL_ADMIN;"
    # query = "select name from syscolumns where id = OBJECT_ID('dbo.tbl_user');"

    database.Connect()
    # database.BeginTransaction()
    # rows = database.GetRow(query)
    # for row in rows:
    #     no = row[0]
    #     id_ = row[8]
    #     pw = row[9]
    #     nickname = row[10]

    #     print('no:% d \nid:% s\npw:% s\nnickname:% s' %
    #           (no, id_, pw, nickname))
    #     print()
    # one_ = database.GetOne(query)
    # print(one_)
    # database.ClearTransaction()
    database.Rollback()
    # database.Execute(query)
    # database.Commit()
    database.Close()


except Exception as e:
    print(e)
