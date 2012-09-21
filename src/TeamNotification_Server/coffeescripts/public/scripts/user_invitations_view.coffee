define 'user_invitations_view', ['general_view'], (GeneralView) ->
    class UserInvitationsView extends GeneralView
    
        id: 'invitations-container'
        
        get_field: (field_name, data) ->
            for field in data
                if field.name is field_name
                    return field.value

        initialize: -> 
        
        resendInvite: (invitation) ->
            alert(invitation)

        render: ->
                
            me = @
            @$el.empty()
            
            @$el.attr('class','row-fluid')
            table = $('<table class="table table-hover table-condensed">')
            table.append "<tr><th>Email</th><th>Room</th><th>Invitation date</th><th></th></tr>"
            if @model.has("invitations")
                $(@model.attributes.invitations).each (idx, invitation) ->
                    accepted = me.get_field('accepted', invitation.data)
                    if accepted == 0
                        email = me.get_field('email', invitation.data)
                        date = me.get_field('date', invitation.data)
                        room = me.get_field('chat_room_name', invitation.data)
                        room_id = me.get_field('chat_room_id', invitation.data)
                        objectData = {email:email, room:room_id}                        
                        button = $("<button value='#{JSON.stringify(objectData)}' class='btn'>Resend Invitation</button>")
                        button.click () ->
                            me.resendInvite button.attr('value')
                        
                        row = $('<tr>')
                        col = $('<td>')
                        row.append "<td>#{email}</td>"
                        row.append "<td>#{room}</td>"
                        row.append "<td>#{date}</td>"
                        col.append button
                        row.append col
                        table.append row
            else
                table.append "There aren't any pending invitations"
            @$el.append table
            console.log @model
            @
