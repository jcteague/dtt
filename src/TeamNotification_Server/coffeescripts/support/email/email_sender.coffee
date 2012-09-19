email_transport = require('./email_transport')

send = (email_message) ->
    email_transport.send_email email_message
    ###
    email_transport.send_email {
        from: 'Invitations <invitations@yacketyapp.com>'
        #to: 'eddy.w.espinal@gmail.com, eespinal@intellisys.com.do'
        to: 'invitations@yacketyapp.com'
        subject: 'Test Email From Nodemailer With AMAZON'
        text: 'AMAZON -> This is a test email from the nodemailer package of node'
        #html: '<b>Hello with Amazon</b>'
    }
    ###

module.exports =
    send: send
