define 'user_invitations_view', ['general_view'], (GeneralView) ->
    class UserInvitationsView extends GeneralView
    
        id: 'invitations-container'
        
        get_field: (field_name, data) ->
            for field in data
                if field.name is field_name
                    return field.value

        initialize: -> 
        
        render: ->
                
            me = @
            @$el.empty()
            
            @$el.attr('class','row-fluid')
            table = $('<table id="invitations-table" class="table table-hover table-condensed">')
            table.append "<tr><th>Email</th><th>Room</th><th>Invitation date</th><th></th></tr>"
            if @model.has("invitations") &&  @model.attributes.invitations.length > 0
                $(@model.attributes.invitations).each (idx, invitation) ->
                    accepted = me.get_field('accepted', invitation.data)
                    if accepted == 0
                        email = me.get_field('email', invitation.data)
                        date = new Date(me.get_field('date', invitation.data)).toUTCString()
                        room = me.get_field('chat_room_name', invitation.data)
                        room_id = me.get_field('chat_room_id', invitation.data)
                        objectData = {email:email, room:room_id}       
                        objectDataSerialized = JSON.stringify(objectData)                 
                        button = $("<button value='#{objectDataSerialized}' class='btn'>Resend Invitation</button>")
                        button.click () ->
                            b = $(@)
                            args = JSON.parse(b.attr('value'))
                            b.attr("disabled", "disabled")
                            $.ajaxSetup
                                beforeSend: (jqXHR) ->
                                    authToken = $.cookie("authtoken")
                                    jqXHR.setRequestHeader('Authorization', authToken )
                                    
                            $.post "room/#{args.room}/users", args, (data) ->
                                b.removeAttr("disabled")
                                $("#server-response-container").append data.messages[0]
                        
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
