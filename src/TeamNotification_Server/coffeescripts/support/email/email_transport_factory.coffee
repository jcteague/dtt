nodemailer = require('nodemailer')
smtp_options = require('./email_configuration')

mock = 
    sendEmail: (email_message, callback) ->
        console.log 'Using Email Transport Mock'
        callback(false, message: 'Using Email Transport Mock')

get = ->
    # TODO: When having set up a correct non production email sending environment, remove this
    return mock if process.env.NODE_ENV is 'test'

    nodemailer.createTransport 'SMTP', smtp_options

module.exports =
    get: get
