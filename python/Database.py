import pymssql


class Database:
    def __init__(self):
        self.conn = None
        self.cur = None
        self.is_tr = False

    def Database(self):
        pass
        # TODO: 생성자 논리를 여기에 추가합니다.

    # def Connection:
    #     return self.conn

    # def Transaction:
    #     return self.tr

    def Connect(self, connectString=None):

        # if connectString is None:
        #     # Connect(ConfigurationManager.AppSettings["ConnectionString"]);
        #     print("DB계정정보가 존재하지 않습니다.")

        # else:
        self.conn = pymssql.connect(server="175.126.82.211", user="anifan",
                                    password="doslvos@anifan11!", database="anifan")
        self.cur = self.conn.cursor()
        print("DB접속 성공")

    def BeginTransaction(self, transactionName=None):
        if transactionName is None:
            self.cur.execute("BEGIN TRANSACTION")
        else:
            self.cur.execute("BEGIN TRANSACTION@%s" %
                             str(transactionName))
        self.is_tr = True
        print("트랜잭션이 시작되었습니다.")

    def ClearTransaction(self, transactionName=None):
        if transactionName is None:
            self.cur.execute("ROLLBACK TRANSACTION")
        else:
            self.cur.execute("ROLLBACK TRANSACTION@%s" %
                             str(transactionName))
        self.is_tr = False
        print('Rollback쿼리가 입력되었습니다.')

    def Rollback(self, transactionName=None):
        if transactionName is None:
            self.conn.rollback()
            self.cur = None
        else:
            str(transactionName)
            self.conn.rollback(transactionName)
        print('Rollback이 실행되었습니다.')

    def Commit(self):
        self.conn.commit()
        print("트랜잭션이 적용되었습니다.")
        rowcount = self.cur.rowcount
        print(rowcount)
        if rowcount is None:
            rowcount = 0
        print("%.0f 개의 행이 영향을 받았습니다." % rowcount)
        return rowcount

    def Execute(self, query):
        str(query)
        self.cur.execute(query)
        if self.is_tr is False:
            self.BeginTransaction()

        # return self.cursor.rowcount
        # rowcount = self.cur.execute("SELECT @@ROWCOUNT")

    def Close(self):
        self.conn.close()
        print("접속 종료")

    def GetRow(self, query):
        str(query)
        self.cur.execute(query)
        if self.is_tr is False:
            self.BeginTransaction()

        self.cur.execute(query)
        result = self.cur.fetchall()

        return result

    def GetOne(self, query):
        str(query)
        self.cur.execute(query)
        if self.is_tr is False:
            self.BeginTransaction()

        self.cur.execute(query)
        result = self.cur.fetchone()

        return result
