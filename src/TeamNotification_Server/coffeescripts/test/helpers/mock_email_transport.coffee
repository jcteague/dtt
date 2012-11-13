nodemailer = require('nodemailer')

get = ->
    obj = 
        path: '/usr/sbin/sendmail'
        args: ['-f test@dtt.com']
    nodemailer.createTransport 'sendmail', obj
    

module.exports =
    get: get
