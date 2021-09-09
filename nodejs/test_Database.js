let Database = require('./Database');
Database = new Database()

const {
  log,
  error
} = console

ConnectionString = {
  "user": "anifan", //default is sa
  "password": "doslvos@anifan11!",
  "server": "175.126.82.211", // for local machine
  "database": "anifan" // name of database
}

query = "select * from TBL_ADMIN"

const Connect = new Promise((resolve, reject) => {
  throw err
  conn = Database.Connect();
  resolve('b')
})

log(Connect)
Connect.then((a) => {
  log(a)
  let result = Database.Execute()
  log(result)
})

// Database.Connect()
// Database.Execute()
// const Connect = async () => {
//   await Database.Connect()
//   return await Database.Execute();
// }

// log(Connect())
// const getData = new Promise()
// getData.then(() => log('성공'))
// getData().then((result) => {
//   log(result)
//   resolve('끝')
//   Database.Close()
// }).catch(function (err) {
//   log(err)
//   // ... error checks
// })

// code = 0;

// function parseJSAsync(script) {
//   return new Promise((resolve, reject) => {
//     const worker = new Worker(__filename, {
//       workerData: script
//     });
//     worker.on("message", resolve);
//     worker.on("error", reject);
//     worker.on("exit", code => {
//       if (code !== 0)
//         reject(new Error(`Worker stopped with exit code ${code}`));
//     });
//   });
// }