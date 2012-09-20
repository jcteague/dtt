nodemailer = require('nodemailer')

get = ->
    nodemailer.createTransport 'sendmail', {
        path: '/usr/sbin/sendmail'
        args: ['-f test@dtt.com']
    }

module.exports =
    get: get
