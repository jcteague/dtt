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
            @members_table = $("""<table class="table table-striped"><tr><th colspan="2">Room members</th></tr></table>""")
            if @model.has('members') and @model.get('members').length > 0
                @members = @model.get('members')
                @render_members(member) for member in @members
            else
                @members_table.append """<tr><td colspan="2">No other members in this room at this moment</td></tr>"""
            @$el.append @members_table
            @
        render_members: (member) ->
            get_field = (data, name) ->
                for field in data
                    if(field.name == name)
                        return field.value
                return ""
            member_name = get_field(member.data,'name')
            
            if(member_name !='')
                me = @
                generate_unsubscribe_link = (room_id, room_name, user_id, user_name, container) ->
                    anchor = $('<a href="#" class="user-unsubscribe-link">Unsubscribe</a>')
                    anchor.bind 'click',() ->
                        del = confirm("Are you sure you want to take '#{user_name}' out of room '#{room_name}'?")
                        if(del)
                            $.post "#{config.api.url}/room/#{room_id}/unsubscribe/#{user_id}", (data) ->
                                me.trigger 'messages:display', data.server_messages 
                                container.hide()
                        return false
                    anchor
                member_row = $("<tr/>")
                
                member_row.append """<td>#{get_field(member.data,'name')}</td>"""
                unsubscribe_column = $("<td/>")
                unsubscribe_room_link = generate_unsubscribe_link get_field(member.data,'room_id'), get_field(member.data,'room_name'), get_field(member.data,'id'), get_field(member.data,'name'), member_row
                unsubscribe_column.append unsubscribe_room_link
                member_row.append unsubscribe_column
                @members_table.append member_row
            
        append_to: (parent) ->
            @$el.appendTo parent
