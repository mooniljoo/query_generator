class Query {
    // init
    nLimitStart = 0;
    nLimitLength = 0;
    strKeyColumn = "[no]";

    strSelectQuery = "";
    strInsertQuery = "";
    strUpdateQuery = "";
    strDeleteQuery = "";
    strTotalRowQuery = "";

    strTable = "";
    strGroup = "";
    strTop = "";
    bDistinct = false;

    aryJoinTable = [];
    aryJoinTableOn = [];
    aryColumn = [];
    aryColumnValue = [];
    aryColumnType = [];
    aryCondition = [];
    aryOrder = [];

Query() {
    this.aryJoinTable = [];
    this.aryJoinTableOn = [];
    this.aryColumn = [];
    this.aryColumnValue = [];
    this.aryColumnType = [];
    this.aryCondition = [];
    this.aryOrder = [];
    this.Clear();
}
Clear() {
    this.nLimitStart = 0;
    this.nLimitLength = 0;
    this.strKeyColumn = "[no]";

    this.strSelectQuery = "";
    this.strInsertQuery = "";
    this.strUpdateQuery = "";
    this.strDeleteQuery = "";
    this.strTotalRowQuery = "";

    this.strTable = "";
    this.strGroup = "";
    this.strTop = "";
    this.bDistinct = false;

    this.aryJoinTable.Clear();
    this.aryJoinTableOn.Clear();
    this.aryColumn.Clear();
    this.aryColumnValue.Clear();
    this.aryColumnType.Clear();
    this.aryCondition.Clear();
    this.aryOrder.Clear();
}

SetTable(table) {
    this.strTable = table;
}

SetGroup(group) {
    this.strGroup = group;
}

SetDistinct() {
    this.bDistinct = true;
}

UnSetDistinct() {
    this.bDistinct = false;
}

AddLeftJoin(join_table) {
    this.aryJoinTable.push(join_table);
    this.aryJoinTableOn.push("");
}

AddLeftJoin(join_table, join_condition) {
    this.aryJoinTable.push(join_table);
    this.aryJoinTableOn.push(join_condition);
}

AddColumn(column_name) {
    this.aryColumn.push(column_name);
    this.aryColumnValue.push("");
    this.aryColumnType.push("value");
}
AddColumn(column_name, column_value) {
    this.aryColumn.push(column_name);
    if (column_value) {
        this.aryColumnValue.push("1");
    }
    else {
        this.aryColumnValue.push("0");
    }
    this.aryColumnType.push("value");
}
AddColumn(column_name, column_value) {
    this.aryColumn.push(column_name);
    this.aryColumnValue.push(column_value.ToString());
    this.aryColumnType.push("value");
}
AddColumn(column_name, column_value) {
    this.aryColumn.push(column_name);
    if (column_value == null || column_value == "")
        this.aryColumnValue.push("");
    else
        this.aryColumnValue.push(column_value.Replace("'", "''"));
    this.aryColumnType.push("value");
}
AddColumn(column_name, column_value, is_function) {
    if (is_function) {
        this.aryColumn.push(column_name);
        this.aryColumnValue.push(column_value);
        this.aryColumnType.push("func");
    }
    else {
        AddColumn(column_name, column_name);
    }
}
AddNVColumn(column_name, column_value) {
    this.aryColumn.push(column_name);
    this.aryColumnValue.push(column_value);
    this.aryColumnType.push("NVARCHAR");
}

AddCondition(condition) {
    this.aryCondition.push(condition);
}

AddOrder(order) {
    this.aryOrder.push(order);
}

SetLimit(start, length) {
    this.nLimitStart = start;
    this.nLimitLength = length;
    this.strKeyColumn = "[no]";
}

SetLimit(start, length, key_column) {
    this.nLimitStart = start;
    this.nLimitLength = length;
    this.strKeyColumn = key_column;
}

TotalRowQuery(bIncludeGroup) {
    this.strTotalRowQuery = "";
    this.strTotalRowQuery += " SELECT COUNT(*) AS total_row ";
    this.strTotalRowQuery += " FROM " + this.strTable;
    this.strTotalRowQuery += this.GetLeftJoinTable();
    this.strTotalRowQuery += this.GetConditionWord();
    if (bIncludeGroup) {
        if (this.strGroup != "") {
            this.strTotalRowQuery += " GROUP BY ";
            this.strTotalRowQuery += this.strGroup;
        }
    }
    return this.strTotalRowQuery;
}

SelectQueryNoLimit() {
    this.strSelectQuery = "";
    this.strSelectQuery += " SELECT ";
    if (this.bDistinct)
        this.strSelectQuery += "DISTINCT ";
    this.strSelectQuery += this.GetColumnWord();
    this.strSelectQuery += " FROM ";
    this.strSelectQuery += this.strTable;
    this.strSelectQuery += this.GetLeftJoinTable();
    this.strSelectQuery += this.GetConditionWord();
    if (this.strGroup != "") {
        this.strSelectQuery += " GROUP BY ";
        this.strSelectQuery += this.strGroup;
    }
    this.strSelectQuery += this.GetOrderWord();

    return this.strSelectQuery;
}

SelectQuery() {
    if (this.strSelectQuery != "") return this.strSelectQuery;

    bLimit = false;
    if (this.nLimitLength == 0 && this.nLimitStart == 0) {
        bLimit = false;
    }
    else {
        bLimit = true;
    }

    this.strSelectQuery = "";

    this.strSelectQuery += " SELECT ";
    if (this.bDistinct)
        this.strSelectQuery += "DISTINCT ";
    if (bLimit)
        this.strSelectQuery += " TOP " + this.nLimitLength + " ";
    this.strSelectQuery += this.GetColumnWord();
    this.strSelectQuery += " FROM ";
    this.strSelectQuery += this.strTable;
    this.strSelectQuery += this.GetLeftJoinTable();
    this.strSelectQuery += this.GetConditionWord();
    if (bLimit) {
        if (this.GetConditionWord() == "") {
            this.strSelectQuery += " WHERE ";
        }
        else {
            this.strSelectQuery += " AND ";
        }
        this.strSelectQuery += " " + this.strKeyColumn
            + " NOT IN (SELECT ";
        if (this.bDistinct)
            this.strSelectQuery += "DISTINCT ";
        this.strSelectQuery += "TOP " + this.nLimitStart + " " + this.strKeyColumn
            + " FROM " + this.strTable + " "
            + this.GetLeftJoinTable() + " "
            + this.GetConditionWord();
        if (this.strGroup != "") {
            this.strSelectQuery += " GROUP BY ";
            this.strSelectQuery += this.strGroup;
        }
        this.strSelectQuery += " " + this.GetOrderWord();
        this.strSelectQuery += ") ";
    }
    if (this.strGroup != "") {
        this.strSelectQuery += " GROUP BY ";
        this.strSelectQuery += this.strGroup;
    }
    this.strSelectQuery += this.GetOrderWord();

    return this.strSelectQuery;
}

SelectFunctionQuery() {
    this.strSelectQuery = "";
    this.strSelectQuery += " SELECT ";
    this.strSelectQuery += this.GetColumnWord();

    return this.strSelectQuery;
}

InsertQuery() {
    if (this.strInsertQuery != "") return this.strInsertQuery;

    var bLimit = false;
    if (this.nLimitLength == 0 && this.nLimitStart == 0) {
        bLimit = false;
    }
    else {
        bLimit = true;
    }

    this.strInsertQuery = "";

    this.strInsertQuery += " INSERT INTO " + this.strTable;

    this.strInsertQuery += "(" + this.GetColumnWord() + ")";

    this.strInsertQuery += " VALUES (" + this.GetColumnValueWord() + ")";

    return this.strInsertQuery;
}

UpdateQuery() {
    if (this.strUpdateQuery != "") return this.strUpdateQuery;

    var bLimit = false;
    if (this.nLimitLength == 0 && this.nLimitStart == 0) {
        bLimit = false;
    }
    else {
        bLimit = true;
    }

    this.strUpdateQuery = "";

    this.strUpdateQuery += " UPDATE " + this.strTable;
    this.strUpdateQuery += " SET";

    if (this.aryColumn.length > 0) {
        for (var i = 0; i < this.aryColumn.length; i++) {
            if (i == 0) {
                if (this.aryColumnType[i] == "func") {
                    this.strUpdateQuery += " " + this.aryColumn[i];
                    this.strUpdateQuery += " = " + this.aryColumnValue[i];
                }
                else if (this.aryColumnType[i] == "NVARCHAR") {
                    this.strUpdateQuery += " " + this.aryColumn[i];
                    this.strUpdateQuery += " = N'" + this.aryColumnValue[i] + "'";
                }
                else {
                    this.strUpdateQuery += " " + this.aryColumn[i];
                    this.strUpdateQuery += " = '" + this.aryColumnValue[i] + "'";
                }
            }
            else {
                if (this.aryColumnType[i] == "func") {
                    this.strUpdateQuery += " ," + this.aryColumn[i];
                    this.strUpdateQuery += " = " + this.aryColumnValue[i];
                }
                else if (this.aryColumnType[i] == "NVARCHAR") {
                    this.strUpdateQuery += " ," + this.aryColumn[i];
                    this.strUpdateQuery += " = N'" + this.aryColumnValue[i] + "'";
                }
                else {
                    this.strUpdateQuery += " ," + this.aryColumn[i];
                    this.strUpdateQuery += " = '" + this.aryColumnValue[i] + "'";
                }
            }
        }
    }

    this.strUpdateQuery += this.GetConditionWord();

    return this.strUpdateQuery;
}

DeleteQuery() {
    if (this.strDeleteQuery != "") return this.strDeleteQuery;

    var bLimit = false;
    if (this.nLimitLength == 0 && this.nLimitStart == 0) {
        bLimit = false;
    }
    else {
        bLimit = true;
    }

    this.strDeleteQuery = "";

    this.strDeleteQuery += " DELETE FROM " + this.strTable;

    this.strDeleteQuery += this.GetConditionWord();

    return this.strDeleteQuery;
}

GetLeftJoinTable() {
    var word = "";
    var table = "";
    var on = "";

    length = this.aryJoinTable.length;
    if (length > 0) {
        for (i = 0; i < length; i++) {
            table = this.aryJoinTable[i];
            word += " LEFT OUTER JOIN " + table + " ";
            if (this.aryJoinTableOn[i] != "") {
                on = this.aryJoinTableOn[i];
                word += " ON " + on + " ";
            }
        }
    }

    return this.word;
}

AddColumn(column_name) {
    this.aryColumn.push(column_name);
    this.aryColumnValue.push("");
    this.aryColumnType.push("value");
}

GetColumnWord() {
    var word = "";
    if (this.aryColumn.length > 0) {
        for (var i = 0; i < this.aryColumn.length; i++) {
            if (word == "") {
                word += this.aryColumn[i];
            }
            else {
                word += ", " + this.aryColumn[i];
            }
        }
    }

    return word;
}

GetColumnValueWord() {
    var word = "";
    if (this.aryColumnValue.length > 0) {
        for (var i = 0; i < this.aryColumnValue.length; i++) {
            var value;
            if (this.aryColumnType[i] == "func") {
                value = this.aryColumnValue[i];
            }
            else if (this.aryColumnType[i] == "NVARCHAR") {
                value = "N'" + this.aryColumnValue[i] + "'";
            }
            else {
                value = "'" + this.aryColumnValue[i] + "'";
            }

            if (word == "")
                word += value;
            else
                word += "," + value;
        }
    }

    return word;
}

//function GetColumnValueWord() {
//    var word = "";
//    if (this.aryColumnValue.length > 0) {
//        for (var i = 0; i < this.aryColumnValue.length; i++) {
//            var value;
//            if (this.aryColumnType[i].Equals("func")) {
//                value = this.aryColumnValue[i];
//            }
//            else if (this.aryColumnType[i].Equals("NVARCHAR")) {
//                value = "N'" + this.aryColumnValue[i] + "'";
//            }
//            else {
//                value = "'" + this.aryColumnValue[i] + "'";
//            }

//            if (word == "")
//                word += value;
//            else
//                word += "," + value;
//        }
//    }

//    return word;
//}

GetConditionWord() {
    var word = "";
    if (this.aryCondition.length > 0) {
        for (var i = 0; i < this.aryCondition.length; i++) {
            if (word == "") {
                word += this.aryCondition[i];
            }
            else {
                word += " AND " + this.aryCondition[i];
            }
        }
        word = " WHERE " + word;
    }
    return word;
}

GetOrderWord() {
    var word = "";
    if (this.aryOrder.length > 0) {
        for (var i = 0; i < this.aryOrder.length; i++) {
            if (word == "") {
                word += this.aryOrder[i];
            }
            else {
                word += ", " + this.aryOrder[i];
            }
        }
        word = " ORDER BY " + word;
    }
    return word;
}
}
