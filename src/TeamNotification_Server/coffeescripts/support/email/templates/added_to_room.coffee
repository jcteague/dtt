config = require('../../../config')()

get_template = (name, room_id, room_name, inviters_name) ->
    room_link = "#{config.site.surl}/#/room/#{room_id}"
    return """
        <div>
            <p>Hi #{name},</p>
            <p>#{inviters_name} has invited you to '#{room_name}', </p>
            <p>
                You can join the conversation at the room by just clicking the link below:
            </p>
            <a href="#{room_link}">#{room_link}</a>
            <br />
            <p>The Yackety App Team</p>
        </div>
    """

using = (fields) ->
    return {
        from: 'User Room Invitation <invitations@yacketyapp.com>'
        to: fields.email
        subject: "Yackety App Room '#{fields.room_name}' Invitation"
        html: get_template(fields.name, fields.room_id, fields.room_name, fields.inviters_name )
    }

module.exports =
    using: using
