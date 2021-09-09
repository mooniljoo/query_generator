
class Query:
    def __init__(self):
        self.nLimitStart = 0
        self.nLimitLength = 0
        self.strKeyColumn = "[no]"

        self.strSelectQuery = ""
        self.strInsertQuery = ""
        self.strUpdateQuery = ""
        self.strDeleteQuery = ""
        self.strTotalRowQuery = ""

        self.strTable = ""
        self.strGroup = ""
        self.strTop = ""
        self.bDistinct = False

        self.aryJoinTable = []
        self.aryJoinTableOn = []
        self.aryColumn = []
        self.aryColumnValue = []
        self.aryColumnType= []
        self.aryCondition = []
        self.aryOrder = []
    
    def Query(self):
        self.Clear()

    def Clear(self):
        self.nLimitStart = 0
        self.nLimitLength = 0
        self.strKeyColumn = "[no]"

        self.strSelectQuery = ""
        self.strInsertQuery = ""
        self.strUpdateQuery = ""
        self.strDeleteQuery = ""
        self.strTotalRowQuery = ""

        self.strTable = ""
        self.strGroup = ""
        self.strTop = ""
        self.bDistinct = False

        self.aryJoinTable = []
        self.aryJoinTableOn = []
        self.aryColumn = []
        self.aryColumnValue = []
        self.aryColumnType= []
        self.aryCondition = []
        self.aryOrder = []

    def SetTable(self, table):
        self.strTable = str(table)

    def SetGroup(self, group):
        self.strGroup = str(group)

    def SetDistinct(self):
        self.bDistinct = True

    def UnSetDistinct(self):
        self.bDistinct = False

    def AddLeftJoin(self, join_table):
        self.aryJoinTable.append(str(join_table))
        self.aryJoinTableOn.append("")

    def AddLeftJoin(self, join_table, join_condition):
        self.aryJoinTable.append(str(join_table))
        self.aryJoinTableOn.append(str(join_condition))

    def AddColumn(self,column_name, column_value,is_function=None):

        if is_function == None:
            self.aryColumn.append(str(column_name))

            if type(column_value) is bool:
                if (bool(column_value)):
                    self.aryColumnValue.append("1")
                else:
                    self.aryColumnValue.append("0")

            if type(column_value) is int:
                self.aryColumnValue.append(str(column_value))

            if type(column_value) is str:
                if str(column_value) == None or str(column_value) == "":
                    self.aryColumnValue.append("")
                else:
                    self.aryColumnValue.append(column_value.replace("'", "''"))            

            self.aryColumnType.append("value")

        else:
            if bool(is_function):
                self.aryColumn.append(str(column_name))
                self.aryColumnValue.append(str(column_value))
                self.aryColumnType.append("func")

            else:
                self.AddColumn(column_name, column_name)

    def AddNVColumn(self,column_name,  column_value):

        self.aryColumn.append(str(column_name))
        self.aryColumnValue.append(str(column_value))
        self.aryColumnType.append("NVARCHAR")

    def AddCondition(self,condition):

        self.aryCondition.append(condition)

    def AddOrder(self,order):

        self.aryOrder.append(str(order))

    def SetLimit(self, start, length, key_column=None):

        self.nLimitStart = int(start)
        self.nLimitLength = int(length)
        
        if key_column == None:
            self.strKeyColumn = "[no]"
        else:
            self.strKeyColumn = str(key_column)

    def TotalRowQuery(self, bIncludeGroup):

        self.strTotalRowQuery = ""
        self.strTotalRowQuery += " SELECT COUNT(*) AS total_row "
        self.strTotalRowQuery += " FROM " + self.strTable
        self.strTotalRowQuery += self.GetLeftJoinTable()
        self.strTotalRowQuery += self.GetConditionWord()
        if bool(bIncludeGroup):

            if self.strGroup != "":

                self.strTotalRowQuery += " GROUP BY "
                self.strTotalRowQuery += self.strGroup

        return self.strTotalRowQuery

    def SelectQueryNoLimit(self):

        self.strSelectQuery = ""
        self.strSelectQuery += " SELECT "
        if self.bDistinct:
            self.strSelectQuery += "DISTINCT "
        self.strSelectQuery += self.GetColumnWord()
        self.strSelectQuery += " FROM "
        self.strSelectQuery += self.strTable
        self.strSelectQuery += self.GetLeftJoinTable()
        self.strSelectQuery += self.GetConditionWord()
        if self.strGroup != "":

            self.strSelectQuery += " GROUP BY "
            self.strSelectQuery += self.strGroup

        self.strSelectQuery += self.GetOrderWord()

        return self.strSelectQuery

    def InsertQuery(self):

        if self.strInsertQuery != "": return self.strInsertQuery

        bLimit = False

        if self.nLimitLength == 0 and self.nLimitStart == 0:

            bLimit = False

        else:
            bLimit = True

        self.strInsertQuery = ""

        self.strInsertQuery += " INSERT INTO " + self.strTable

        self.strInsertQuery += "(" + self.GetColumnWord() + ")"

        self.strInsertQuery += " VALUES (" + self.GetColumnValueWord() + ")"

        return self.strInsertQuery

    def UpdateQuery(self):
        if (self.strUpdateQuery != ""): return self.strUpdateQuery

        bLimit = False
        if (self.nLimitLength == 0 and self.nLimitStart == 0):
            bLimit = False
        else:
            bLimit = True

        self.strUpdateQuery = ""

        self.strUpdateQuery += " UPDATE " + self.strTable
        self.strUpdateQuery += " SET"

        if len(self.aryColumn) > 0:
            for i in range(len(self.aryColumn)):
                if i == 0:
                    if str(self.aryColumnType[i]) == "func":
                        self.strUpdateQuery += " " + str(self.aryColumn[i])
                        self.strUpdateQuery += " = " + str(self.aryColumnValue[i])
                    elif str(self.aryColumnType[i]) == "NVARCHAR":
                        self.strUpdateQuery += " " + str(self.aryColumn[i])
                        self.strUpdateQuery += " = N'" + str(self.aryColumnValue[i]) + "'"
                    else:
                        self.strUpdateQuery += " " + str(self.aryColumn[i])
                        self.strUpdateQuery += " = '" + str(self.aryColumnValue[i]) + "'"
                else:
                    if str(self.aryColumnType[i]) == "func":
                        self.strUpdateQuery += " ," + str(self.aryColumn[i])
                        self.strUpdateQuery += " = " + str(self.aryColumnValue[i])
                    elif str(self.aryColumnType[i]) == "NVARCHAR":
                        self.strUpdateQuery += " ," + str(self.aryColumn[i])
                        self.strUpdateQuery += " = N'" + str(self.aryColumnValue[i]) + "'"
                    else:
                        self.strUpdateQuery += " ," + str(self.aryColumn[i])
                        self.strUpdateQuery += " = '" + str(self.aryColumnValue[i]) + "'"

        self.strUpdateQuery += self.GetConditionWord()

        return self.strUpdateQuery

    def DeleteQuery(self):
        if self.strDeleteQuery != "": return self.strDeleteQuery

        bLimit = False
        if self.nLimitLength == 0 and self.nLimitStart == 0:
            bLimit = False
        else:
            bLimit = True

        self.strDeleteQuery = ""

        self.strDeleteQuery += " DELETE FROM " + self.strTable

        self.strDeleteQuery += self.GetConditionWord()

        return self.strDeleteQuery

    def GetLeftJoinTable(self):
        word = ""
        table = ""
        on = ""
        length = len(self.aryJoinTable)
        if length > 0:
            for i in len(length):
                table = str(self.aryJoinTable[i])
                word += " LEFT OUTER JOIN " + table + " "
                if (str(self.aryJoinTableOn[i]) != ""):
                    on = str(self.aryJoinTableOn[i])
                    word += " ON " + on + " "
        return word

    def GetColumnWord(self):
        word = ""        
        if len(self.aryColumn)> 0:
            for i in range(len(self.aryColumn)):
                if word == "":
                    word += str(self.aryColumn[i])
                else:
                    word += ", " + str(self.aryColumn[i])
        return word

    def GetColumnValueWord(self):
        word = ""
        if len(self.aryColumnValue) > 0:
            for i in range(len(self.aryColumnValue)):
                value = ""
                if self.aryColumnType[i] is "func":
                    value = str(self.aryColumnValue[i])
                elif self.aryColumnType[i] is "NVARCHAR":
                    value = "N'" + str(self.aryColumnValue[i]) + "'"
                else:
                    value = "'" + str(self.aryColumnValue[i]) + "'"
                if word == "":
                    word += value
                else:
                    word += "," + value

        return word

    def GetConditionWord(self):
        word = ""
        if len(self.aryCondition) > 0:
            for i in range(len(self.aryCondition)):
                if word == "":
                    word += str(self.aryCondition[i])
                else:
                    word += " AND " + self.aryCondition[i]
                    
            word = " WHERE " + word
        return word

    def GetOrderWord(self):
        word = ""
        if len(self.aryOrder) > 0:
            for i in range(len(self.aryOrder)):
                if word == "":
                    word += str(self.aryOrder[i])
                else:
                    word += ", " + str(self.aryOrder[i])
            word = " ORDER BY " + word
        return word