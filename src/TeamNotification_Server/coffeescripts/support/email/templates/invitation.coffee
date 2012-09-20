config = require('../../../config')()

get_template = (chat_room) ->
    join_room_link = "#{config.site.url}/room/#{chat_room.id}/accept-invitation"
    return """
        <div>
            <p>Hi,</p>
            <br />
            <p>You have been invited to join a room in Yackety App</p>
            <p>
                We are waiting for you on <b>#{chat_room.name}</b>. If you want to join, just click on the link below:
            </p>
            <br />
            <a href="#{join_room_link}">#{join_room_link}</a>
            <br />
            <p>The Yackety App Team</p>
        </div>
    """

using = (email_invitation_values) ->
    return {
        from: 'Invitations <invitations@yacketyapp.com>'
        to: email_invitation_values.email
        subject: 'You have been invited to join a room in Yackety App'
        html: get_template(email_invitation_values.chat_room)
    }

module.exports =
    using: using
