db = require('./db_helper')

structure = 
    name: 'users'
    columns:
        id: 'integer'
        name: 'varchar(100)'
        email: 'varchar(100)'

blah_struct = 
    name: 'blahs'
    columns:
        id: 'integer'
        name: 'varchar(100)'

users =
    name: 'users'
    entities: [
        {
            id: 1
            name: "'blah'"
            email: "'foo@bar.com'"
        },
        {
            id: 2
            name: "'ed2'"
            email: "'ed@es.com'"
        }
    ]

blahs =
    name: 'blahs'
    entities: [
        {
            id: 1
            name: "'blah'"
        },
        {
            id: 2
            name: "'ed2'"
        }
    ]

db.handle db.clear('users', 'blahs'), db.create(structure, blah_struct), db.save(users, blahs)
