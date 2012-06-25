Sequelize = require('sequelize')
db_config = require('../globals').db

sequelize = new Sequelize db_config.test, db_config.user, db_config.password, {
    dialect: 'postgres'
}

User = sequelize.define 'User', {
        id: {type: Sequelize.INTEGER, primaryKey: true}
        name: Sequelize.STRING
        email: Sequelize.STRING
    }, {
        underscored: true
    }

ChatRoom = sequelize.define 'ChatRoom', {
        name: Sequelize.STRING
    }, {
        underscored: true
    }

ChatRoom.hasOne User, {as: 'Owner'}
ChatRoom.hasMany User, {as: 'Users'}

User.sync()
ChatRoom.sync()
