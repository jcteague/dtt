define 'chat_room_view', ['general_view','messages_view','form_view','user_rooms_view','room_users_view','bootstrap','bootstrap-dropdown','bootstrap-collapse'], (GeneralView, MessagesView, FormView, UserRoomsView,RoomUsersView,Bootstrap,Bootstrap_Dropdown,Bootstrap_Collapse) ->

    class ChatRoomView extends GeneralView

        id: 'chatroom-container'

        events:
            'keydown textarea': 'keydown_textarea'

        initialize: ->
            @user_rooms = new UserRoomsView(model:@model)
            @form_view = new FormView(model: @model)
            @message_view = new MessagesView(model: @model)
            @room_users_view = new RoomUsersView(model: @model)

        render: ->
            @$el.empty()
            $("#client-content").attr('class','')
            @$el.attr('class', 'row-fluid')
            div1 = $('<div class="row-fluid">')
            @user_rooms.render().append_to div1
            @$el.append div1
            div2 = $('<div class="row-fluid">')
            @message_view.render().append_to div2
            @room_users_view.render().append_to div2
            @$el.append div2
            div3 = $('<div class="row-fluid">')
            div3.append("""<span class='inline'><input type='checkbox' id='chk_mute'/><b>Mute</b></span>""")
            @form_view.render().append_to div3
            #@form_view.$el.append("""<input type='checkbox' id='chk_mute'/> Mute""") 
            div3.find('form').attr('class','well form-inline')
            div3.find('label').attr('style','vertical-align:middle;')
            div3.find('input[type=submit]').attr('class','btn btn-primary btn-large')
            div3.find('textarea').width('85%')
            
            @$el.append(div3)

            set_height = () ->
                winHeight = $(window).height()
                $(div3).attr('style','height:'+(winHeight/7)+'px')
                div2.find('div').each (key, value) ->
                    newHeight = ( (winHeight*5)/7)+'px'
                    $(value).attr('style','height:'+newHeight)
            set_height()
            $(window).resize set_height
            @

        append_to: (parent) ->
            @$el.appendTo parent

        keydown_textarea: (event) ->
            textarea = $(event.currentTarget)
            key = if event.keyCode? then event.keyCode else event.which
            if key is @ENTER and not event.shiftKey
                @$('input[type=submit]').click()
                event.preventDefault()

        ENTER: 13
