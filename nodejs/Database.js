let mssql = require('mssql');

const {
    log,
    error
} = console


class Database {
    Database() {

        // TODO: 생성자 논리를 여기에 추가합니다.

    }
    Connect() {
        // if (connectString == null || connectString == '') {
        //     Connect(ConfigurationManager.AppSettings["ConnectionString"]);
        //     print("DB계정정보가 존재하지 않습니다.")
        // } else {
        mssql.connect(ConnectionString, err => {
            if (err) {
                log(err, "DB 연결 실패")
            }
            log("DB 연결 성공");
            // this.Execute()
        });

        // }
    }
    Execute() {
        const req = new mssql.Request()
        req.query(query, (err, result) => {
            if (err) {
                log(err, "쿼리 전송 실패")
            }
            log(result)
            return result
        })
    }
    BeginTransaction(transactionName) {
        if (transactionName == null || transactionName == '') {
            var transaction = new mssql.Transaction(globalConnection);
            transaction.begin(function (err) {
                if (err) {
                    log(err, "트랜잭션 실행에 실패했습니다.");
                }

                log("트랜잭션이 시작되었습니다.");
                var request = new mssql.Request(transaction);
                request.query(query, function (err, recordset) {
                    if (err) {
                        log(err, "쿼리 전송에 실패했습니다.");
                    }
                    this.Commit()

                });
            });
            req.results = recordset;
            next();
        } else {
            log("트랜잭션 이름이 있을 때 함수 미기재");
        }
    }

    Commit() {
        transaction.commit(function (err, recordset) {
            if (err) {
                log(err, "트랜잭션 적용에 실패했습니다.");
            }
            log("트랜잭션이 적용되었습니다.");
        });

    }
    Close() {
        mssql.close()
        log("DB 접속 종료")
    }

    error() {

        mssql.on('error', err => {
            // ... error handler 
            log(err, "Sql database connection error ");
        })
    }
}


module.exports = Database;