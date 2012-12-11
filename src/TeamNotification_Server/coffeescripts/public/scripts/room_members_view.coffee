define 'room_members_view', ['general_view','config'], (GeneralView, config) ->

    class RoomMembersView extends GeneralView

        id: 'room-members-container'
        
        initialize: ->
            authToken = $.cookie("authtoken")
            $.ajaxSetup
                beforeSend: (jqXHR) ->
                    jqXHR.setRequestHeader('Authorization', authToken )
                    jqXHR.withCredentials = true
        
        render: ->
            @$el.empty()
            if @model.has('members') and @model.get('members').length > 0
                @$el.attr('class','hero-unit')
                @members = @model.get('members')
                @$el.append('<h2>Room Members</h2>')
                @render_members(member) for member in @members
            @
        render_members: (member) ->
            get_field = (data, name) ->
                for field in data
                    if(field.name == name)
                        return field.value
                return ""
            member_name = get_field(member.data,'name')
            
            if(member_name !='')
                generate_unsubscribe_link = (room_id, room_name, user_id, user_name, container) ->
                    anchor = $('<a href="#" class="user-unsubscribe-link">Unsubscribe</a>')
                    anchor.bind 'click',() ->
                        del = confirm("Are you sure you want to take '#{user_name}' out of room '#{room_name}'?")
                        if(del)
                            $.post "#{config.api.url}/room/#{room_id}/unsubscribe/#{user_id}", (data) ->
                                $("#server-response-container").html(data.server_messages[0])
                                container.hide()
                        return false
                    anchor
                p = $("<p/>")
                p.append """<a href="">#{get_field(member.data,'name')}</a>"""
                p.append " - "
                unsubscribe_room_link = generate_unsubscribe_link get_field(member.data,'room_id'), get_field(member.data,'room_name'), get_field(member.data,'id'), get_field(member.data,'name'), p
                p.append unsubscribe_room_link
                @$el.append p
            
        append_to: (parent) ->
            @$el.appendTo parent
