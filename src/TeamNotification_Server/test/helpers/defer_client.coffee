db = require('./db_helper')

structure = 
    name: 'users'
    columns:
        id: 'integer'
        name: 'varchar(100)'
        email: 'varchar(100)'

users =
    name: 'users'
    entities: [
        {
            id: 1
            name: 'blah'
            email: 'foo@bar.com'
        },
        {
            id: 2
            name: 'ed'
            email: 'ed@es.com'
        }
    ]

###
open = db.open()
console.log open

clear = open.then(db.clear('users'))
console.log clear

create = clear.then(db.create(structure))
console.log create
    
save = create.then(db.save(users))
console.log save

save.then(() -> console.log 'done')
###
db.open_all db.clear('users'), db.create(structure), db.save(users)
