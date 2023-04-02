(function () {
var DXDemo = {};

DXDemo.focusSearch = function() {
    SearchBox.Focus();
};

DXDemo.CurrentDemoGroupKey = "";
DXDemo.CurrentDemoKey = "";
DXDemo.CurrentThemeCookieKey = "DXCurrentTheme";
DXDemo.CodeLoaded = false;
DXDemo.ShowCodeInNewWindow = function(demoTitle, codeTitle){
    var codeWnd = window.open("", "_blank", "status=0,toolbar=0,scrollbars=1,resizable=1,top=74,left=74,width=" + (screen.availWidth - 150) + ",height=" + (screen.availHeight - 150));
    codeWnd.document.open();
    codeWnd.document.write("<html><head><title>");
    codeWnd.document.write(demoTitle + " - " + codeTitle);
    codeWnd.document.write("</title>");
    var links = document.getElementsByTagName("link");
    for(var i = 0; i < links.length; i++){
        if(links[i].href && links[i].href.indexOf("CodeFormatter.css") > -1){
            codeWnd.document.write("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + links[i].href + "\" />");
            break;
        }
    }
    codeWnd.document.write("</head><body><code>");
    var codeTab = DemoPageControl.GetTabByName(codeTitle);
    codeWnd.document.write(document.getElementById("CodeBlock" + (codeTab.index - 1)).innerHTML);
    codeWnd.document.write("</code></body></html>");
    codeWnd.document.close();
}
DXDemo.SetCurrentTheme = function(theme){
    ThemeSelectorPopup.Hide();
    ASPxClientUtils.SetCookie(DXDemo.CurrentThemeCookieKey, theme);
    if(document.forms[0] && (!document.forms[0].onsubmit || document.forms[0].onsubmit.toString().indexOf("Sys.Mvc.AsyncForm") == -1)) {
        // for export purposes
        var eventTarget = document.getElementById("__EVENTTARGET");
        if(eventTarget)
            eventTarget.value = "";
        var eventArgument = document.getElementById("__EVENTARGUMENT");
        if(eventArgument)
            eventArgument.value = "";

        document.forms[0].submit();
    } else {
        window.location.reload();
    }
}
DXDemo.CodeNavBar_HeaderClick = function(s, e) {
    var source = ASPx.Evt.GetEventSource(e.htmlEvent);
    var tag = source.tagName;
    e.cancel = tag != "A" && tag != "IMG";
}
DXDemo.ShowScreenshotWindow = function(evt, link, useExtendedPopup){
    DXDemo.ShowScreenshot(link.href, useExtendedPopup);
    evt.cancelBubble = true;
    return false;
}
DXDemo.ShowScreenshot = function(src, useExtendedPopup){
	var getPopupFunc = useExtendedPopup ? DXDemo.getScreenshotExtendedPopup : DXDemo.getScreenshotPopup;
    DXDemo.ShowScreenshotCore(src, getPopupFunc);
}
DXDemo.ShowScreenshotCore = function(src, getPopupFunc){
    var screenLeft = document.all && !document.opera ? window.screenLeft : window.screenX;
	var screenWidth = screen.availWidth;
	var screenHeight = screen.availHeight;
    var zeroX = Math.floor((screenLeft < 0 ? 0 : screenLeft) / screenWidth) * screenWidth;

	var windowWidth = 475;
	var windowHeight = 325;
	var windowX = parseInt((screenWidth - windowWidth) / 2);
	var windowY = parseInt((screenHeight - windowHeight) / 2);
	if(windowX + windowWidth > screenWidth)
		windowX = 0;
	if(windowY + windowHeight > screenHeight)
		windowY = 0;

    windowX += zeroX;

    var popupwnd = getPopupFunc(src, windowX, windowY, windowWidth, windowHeight);
	if (popupwnd != null && popupwnd.document != null && popupwnd.document.body != null) {
	    popupwnd.document.body.style.margin = "0px";
    }
}
DXDemo.getScreenshotPopup = function(src, windowX, windowY, windowWidth, windowHeight, showScrollbars) {
    var scrollbars = showScrollbars ? "yes" : "no";
    return window.open(src, '_blank', "left=" + windowX + ",top=" + windowY + ",width=" + windowWidth + ",height=" + windowHeight + ", scrollbars=" + scrollbars + ", resizable=no", true);
}
DXDemo.getScreenshotExtendedPopup = function(src, windowX, windowY, windowWidth, windowHeight) {
    var popup = DXDemo.getScreenshotPopup("", windowX, windowY, windowWidth, windowHeight, true);
    var doc = popup.document,
        img = doc.createElement("img");

    img.src = src;
    img.style.width = "100%";
    img.style.height = "100%";
    doc.body.appendChild(img);
    return popup;
}
DXDemo.treeViewNodeClick = function(s, e) {
    var node = e.node;
    if(node.GetNavigateUrl() == "javascript:;") {
        node.SetExpanded(!node.GetExpanded());
    }
}
DXDemo.treeViewExpandedChanging = function(s, e) {
    var node = e.node;
    if(!node.parent && s.GetNodeCount() == 1)
        e.cancel = true;
}


DXDemo.Search = {
    searchTimeout: null,
    lastSearch: null,
    selectedItem: null,
    isInCallback: false,

    onSearchBoxKeyPress: function(s, e) {
        var text = SearchBox.GetValue();
        if(text != DXDemo.Search.lastSearch) {
            if(DXDemo.Search.searchTimeout != null)
                clearTimeout(DXDemo.Search.searchTimeout);
            DXDemo.Search.searchTimeout = setTimeout(function() {
                DXDemo.Search.doSearch(text);
            }, 250);
        }
        DXDemo.Search.lastSearch = text;
    },
    onSearchBoxKeyDown: function(s, e) {
        var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
        var prevent = false;
        if(keyCode == 13) {
            ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
            if(DXDemo.Search.selectedItem)
                document.location = DXDemo.Search.selectedItem.GetNavigateUrl();
        }
        if(keyCode == 40) {
            DXDemo.Search.moveSelectedItem(1);
            prevent = true;
        }
        if(keyCode == 38) {
            DXDemo.Search.moveSelectedItem(-1);
            prevent = true;
        }
        if(prevent)
            ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
    },
    onSearchBoxGotFocus: function() {
        var text = SearchBox.GetValue();
        if(text && text.length > 2)
            DXDemo.Search.showPopup();
    },

    doSearch: function(text) {
        DXDemo.Search.selectedItem = null;
        if(text && text.length > 2) {
            DXDemo.Search.showPopup();
            var isMvc = window.MVCxClientPopupControl !== undefined && SearchPopup instanceof window.MVCxClientPopupControl;
            SearchPopup.PerformCallback(isMvc ? { text: text} : text);
        }
        else {
            if(window.SearchResultsNavBar && SearchResultsNavBar.GetMainElement()) {
                var groupCount = SearchResultsNavBar.GetGroupCount();
                for(var i = 0; i < groupCount; i++)
                    SearchResultsNavBar.GetGroup(i).SetVisible(false);
            }
            SearchPopup.Hide();
        }
    },
    onSearchPopupBeginCallback: function() {
        DXDemo.Search.isInCallback = true;
    },
    onSearchPopupEndCallback: function() {
        DXDemo.Search.isInCallback = false;
    },
    moveSelectedItem: function(direction) {
        if(!window.SearchResultsNavBar)
            return;
        var groupCount = SearchResultsNavBar.GetGroupCount();
        var group, item;
        var newItem = null;
        if(groupCount == 0 || !SearchPopup.IsVisible() || DXDemo.Search.isInCallback) {
            DXDemo.Search.selectedItem = null;
            return;
        }
        if(DXDemo.Search.selectedItem == null) {
            group = SearchResultsNavBar.GetGroup(direction > 0 ? 0 : groupCount - 1);
            item = group.GetItem(direction > 0 ? 0 : group.GetItemCount() - 1);
            newItem = item;
        }
        else {
            group = DXDemo.Search.selectedItem.group;
            if(direction > 0) {
                if(DXDemo.Search.selectedItem.index < group.GetItemCount() - 1)
                    newItem = group.GetItem(DXDemo.Search.selectedItem.index + 1);
                else {
                    group = SearchResultsNavBar.GetGroup(group.index < groupCount - 1 ? group.index + 1 : 0);
                    newItem = group.GetItem(0);
                }
            }
            else {
                if(DXDemo.Search.selectedItem.index > 0)
                    newItem = group.GetItem(DXDemo.Search.selectedItem.index - 1);
                else {
                    group = SearchResultsNavBar.GetGroup(group.index > 0 ? group.index - 1 : groupCount - 1);
                    newItem = group.GetItem(group.GetItemCount() - 1);
                }
            }
        }
        DXDemo.Search.selectNavBarItem(newItem);
    },
    showPopup: function() {
        DXDemo.Search.selectNavBarItem(null);
        SearchPopup.ShowAtElementByID("SearchPanel");
    },
    selectNavBarItem: function(item) {
        if(window.SearchResultsNavBar && SearchResultsNavBar.GetMainElement()) {
            SearchResultsNavBar.SetSelectedItem(item);
            DXDemo.Search.selectedItem = item;
            if(item) {
                var scrollContainer = SearchResultsNavBar.GetMainElement().parentNode;
                var itemElement = document.getElementById("sr_" + item.group.index + "_" + item.index);
                var scrollContainerHeight = scrollContainer.clientHeight;
                var itemElementOffsetTop = itemElement.offsetTop;
                var topOverlap = scrollContainer.scrollTop - itemElementOffsetTop;
                if(topOverlap > 0)
                    scrollContainer.scrollTop -= topOverlap;
                else {
                    var bottomOverlap = itemElementOffsetTop + itemElement.offsetHeight - scrollContainer.scrollTop - scrollContainerHeight;
                    if(bottomOverlap > 0)
                        scrollContainer.scrollTop += bottomOverlap;
                }
            }
        }
    }
}

var DXEventMonitor = {
    TimerId: -1,
    PendingUpdates: [ ],

    Trace: function(sender, e, eventName, encodeHtml) {
        var eventListContainer = document.getElementById("EventListContainer");
        if(eventListContainer) {
            var checkbox = document.getElementById("EventCheck" + eventName);
            if(!checkbox.checked) return;
        }
        var self = DXEventMonitor;
        var name = sender.name;
        var lastSeparator = name.lastIndexOf("_");
        if(lastSeparator > -1)
            name = name.substr(lastSeparator + 1);

        var builder = ["<table>"];
        self.BuildTraceRowHtml(builder, "Sender", name, 100);
        self.BuildTraceRowHtml(builder, "EventType", eventName.replace(/_/g, " "));
        var count = 0;
        for(var child in e) {
            if (typeof e[child] == "function") continue;
            var childValue = e[child];
            if (typeof e[child] == "object" && e[child] && e[child].name)
                childValue = "[name: '" + e[child].name + "']";
            if (encodeHtml)
                childValue = self.EscapeHtml(childValue);
            self.BuildTraceRowHtml(builder, count ? "" : "Arguments", child + " = " + childValue);
            count++;
        }
        builder.push("</table><br />");

        self.PendingUpdates.unshift(builder.join(""));
        if(self.TimerId < 0)
            self.TimerId = window.setTimeout(self.Update, 0);
    },

    BuildTraceRowHtml: function(builder, name, text, width) {
        builder.push("<tr><td valign=top");
        if(width)
            builder.push(" style='width: ", width, "px'");
        builder.push(">");
        if(name)
            builder.push("<b>", name, ":</b>");
        builder.push("</td><td valign=top>", text, "</td></tr>");
    },

    GetLogElement: function() {
        return document.getElementById("EventLog")
    },

    Update: function() {
        var self = DXEventMonitor;
        var element = self.GetLogElement();

        element.innerHTML = self.PendingUpdates.join("") + element.innerHTML;
        self.TimerId = -1;
        self.PendingUpdates = [ ];
    },

    Clear: function() {
       DXEventMonitor.GetLogElement().innerHTML = "";
    },

    EscapeHtml: function(str) {
        return str.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }
};

var DXUploadedFilesContainer = {
    nameCellStyle: "",
    sizeCellStyle: "",
    useExtendedPopup: false,

    AddFile: function(fileName, fileUrl, fileSize) {
        var self = DXUploadedFilesContainer;
        var builder = ["<tr>"];

        builder.push("<td class='nameCell'");
        if(self.nameCellStyle)
            builder.push(" style='" + self.nameCellStyle + "'");
        builder.push(">");
        self.BuildLink(builder, fileName, fileUrl);
        builder.push("</td>");

        builder.push("<td class='sizeCell'");
        if(self.sizeCellStyle)
            builder.push(" style='" + self.sizeCellStyle + "'");
        builder.push(">");
        builder.push(fileSize);
        builder.push("</td>");

        builder.push("</tr>");

        var html = builder.join("");
        DXUploadedFilesContainer.AddHtml(html);
    },
    Clear: function() {
        DXUploadedFilesContainer.ReplaceHtml("");
    },
    BuildLink: function(builder, text, url) {
        builder.push("<a target='blank' onclick='return DXDemo.ShowScreenshotWindow(event, this, " + this.useExtendedPopup + ");'");
        builder.push(" href='" + url + "'>");
        builder.push(text);
        builder.push("</a>");
    },
    AddHtml: function(html) {
        var fileContainer = document.getElementById("uploadedFilesContainer"),
            fullHtml = html;
        if(fileContainer) {
            var containerBody = fileContainer.tBodies[0];
            fullHtml = containerBody.innerHTML + html;
        }
        DXUploadedFilesContainer.ReplaceHtml(fullHtml);
    },
    ReplaceHtml: function(html) {
        var builder = ["<table id='uploadedFilesContainer' class='uploadedFilesContainer'><tbody>"];
        builder.push(html);
        builder.push("</tbody></table>");
        var contentHtml = builder.join("");
        FilesRoundPanel.SetContentHtml(contentHtml);
    },
    ApplySettings: function(nameCellStyle, sizeCellStyle, useExtendedPopup) {
        var self = DXUploadedFilesContainer;
        self.nameCellStyle = nameCellStyle;
        self.sizeCellStyle = sizeCellStyle;
        self.useExtendedPopup = useExtendedPopup;
    }
};

DXDemo.HightlightedCode = {
    TOP_OFFSET_FOR_TAB_CONTROL: 10,
    BOTTOM_PADDING_FOR_CODE: 20,

    visitedTabArrayIndexes: [ ],

    GetPageControlOffsetTop: function() {
        return DemoPageControl.GetMainElement().offsetTop;
    },

    StartScrollAnimation: function(from, to, documentScrollY) {
        var duration = to - from < 350 ? 300 : 700;

        ASPx.AnimationHelper.createSimpleAnimationTransition({
            duration: duration,
            transition: ASPx.AnimationConstants.Transitions.SINE,
            onUpdate: function (value) {
                documentScrollY += value;
                if(ASPx.Browser.MacOSMobilePlatform) // TODO: B157267
                    document.body.scrollTop = documentScrollY;
                else
                    ASPx.SetDocumentScrollTop(documentScrollY);
            }
        }).Start(from, to);
    },

    DemoPageControl_OnActiveTabChanged: function(s, e) {
        if(!DXDemo.HightlightedCode.visitedTabArrayIndexes[s.GetActiveTabIndex()]) {
            if(e.tab.name != "Description") {
                var pageControlElement = DemoPageControl.GetMainElement(),
                    topElementOffset = DXDemo.HightlightedCode.GetPageControlOffsetTop(),
                    topScreenOffset = DXDemo.HightlightedCode.TOP_OFFSET_FOR_TAB_CONTROL,

                    scrollDistance = 0,
                    documentScrollY = 0,
                    scrollTop = ASPx.Browser.MacOSMobilePlatform ? document.body.scrollTop : ASPx.GetDocumentScrollTop(); // TODO: B157267

                var pageHeightBelowScreen = document.body.scrollHeight - scrollTop - window.innerHeight;
                var endCodeContainer = pageControlElement.offsetTop + pageControlElement.offsetHeight - window.innerHeight;
                scrollDistance = topElementOffset - topScreenOffset > pageHeightBelowScreen ? endCodeContainer : topElementOffset - topScreenOffset;

                if(scrollTop > scrollDistance)
                    scrollDistance += scrollTop;

                var pageControlBottomHeight = pageControlElement.offsetTop + pageControlElement.offsetHeight;
                var bottomOfScreenOnPage = scrollTop + window.innerHeight + DXDemo.HightlightedCode.BOTTOM_PADDING_FOR_CODE;

                if(pageControlBottomHeight > bottomOfScreenOnPage)
                    DXDemo.HightlightedCode.StartScrollAnimation(scrollTop, scrollDistance, documentScrollY);
            }
            DXDemo.HightlightedCode.visitedTabArrayIndexes[s.GetActiveTabIndex()] = true;
        }
    }
};

DXDemo.HightlightedCode = {
        TOP_OFFSET_FOR_TAB_CONTROL: 10,
        TOP_OFFSET_FOR_HIGHLIGHTED_BLOCK: 90,
        SCROLL_CODE_BOTTOM_PADDING: 20,
        BOTTOM_PADDING_FOR_ASPX_CODE: 100,
        BOTTOM_PADDING_FOR_CODE: 20,

        visitedTabArrayIndexes: [ ],

        StartScrollAnimation: function(from, to, documentScrollY) {
        var duration = to - from < 350 ? 300 : 700;

        ASPx.AnimationHelper.createSimpleAnimationTransition({
            duration: duration,
            transition: ASPx.AnimationConstants.Transitions.SINE,
            onUpdate: function (value) {
                documentScrollY += value;
                if(ASPx.Browser.MacOSMobilePlatform) // TODO: B157267
                    document.body.scrollTop = documentScrollY;
                else
                    ASPx.SetDocumentScrollTop(documentScrollY);
            }
        }).Start(from, to);
    },

    GetPageControlOffsetTop: function() {
        return DemoPageControl.GetMainElement().offsetTop;
    },

    GetFileExtension: function(fileName) {
        var index = fileName.lastIndexOf('.');
        return index != -1 ? fileName.substr(index + 1) : "";
    },

    DemoPageControl_OnActiveTabChanged: function(s, e) {
        if(!DXDemo.HightlightedCode.visitedTabArrayIndexes[s.GetActiveTabIndex()]) {
            if(e.tab.name != "Description") {
                var isASPXtab = e.tab.name == "ASPX" || DXDemo.HightlightedCode.GetFileExtension(e.tab.name) == "aspx",
                    pageControlElement = DemoPageControl.GetMainElement(),
                    topElementOffset = DXDemo.HightlightedCode.GetPageControlOffsetTop(),
                    elementOffsetHeight = pageControlElement.offsetHeight,
                    topScreenOffset = DXDemo.HightlightedCode.TOP_OFFSET_FOR_TAB_CONTROL,

                    scrollDistance = 0,
                    documentScrollY = 0,
                    highlightedDivOffsetY = 0,
                    scrollTop = ASPx.Browser.MacOSMobilePlatform ? document.body.scrollTop : ASPx.GetDocumentScrollTop(); // TODO: B157267

                if(isASPXtab) {
                    var highlightedBlocks = ASPx.GetNodesByClassName(document.documentElement, "HighlightedBlock");
                    if(highlightedBlocks.length > 0) {
                        topElementOffset = ASPx.GetAbsoluteY(highlightedBlocks[0]);
                        elementOffsetHeight = highlightedBlocks[0].offsetHeight;
                        topScreenOffset = DXDemo.HightlightedCode.TOP_OFFSET_FOR_HIGHLIGHTED_BLOCK;
                        highlightedDivOffsetY = topElementOffset;
                    }
                }

                if(topElementOffset - DXDemo.HightlightedCode.GetPageControlOffsetTop() + DXDemo.HightlightedCode.BOTTOM_PADDING_FOR_ASPX_CODE < window.innerHeight) {
                    topElementOffset = DXDemo.HightlightedCode.GetPageControlOffsetTop();
                    topScreenOffset = DXDemo.HightlightedCode.TOP_OFFSET_FOR_TAB_CONTROL;
                } else if (topElementOffset < window.innerHeight) {
                    topElementOffset = elementOffsetHeight;
                    topScreenOffset = DXDemo.HightlightedCode.TOP_OFFSET_FOR_TAB_CONTROL;
                }

                var pageHeightBelowScreen = document.body.scrollHeight - scrollTop - window.innerHeight;
                var endCodeContainer = pageControlElement.offsetTop + pageControlElement.offsetHeight - window.innerHeight;
                if(elementOffsetHeight < window.innerHeight && highlightedDivOffsetY - DXDemo.HightlightedCode.GetPageControlOffsetTop() > window.innerHeight)
                    scrollDistance = highlightedDivOffsetY + elementOffsetHeight - window.innerHeight + DXDemo.HightlightedCode.SCROLL_CODE_BOTTOM_PADDING;
                else
                    scrollDistance = topElementOffset - topScreenOffset > pageHeightBelowScreen ? endCodeContainer : topElementOffset - topScreenOffset;

                if(scrollTop > scrollDistance)
                    scrollDistance += scrollTop;

                var highlightedDivBottomY = highlightedDivOffsetY + elementOffsetHeight;
                var pageControlBottomY = pageControlElement.offsetTop + pageControlElement.offsetHeight;
                var bottomOfScreenOnPage = scrollTop + window.innerHeight + DXDemo.HightlightedCode.BOTTOM_PADDING_FOR_CODE;

                if(isASPXtab && highlightedDivBottomY > bottomOfScreenOnPage || pageControlBottomY > bottomOfScreenOnPage)
                    DXDemo.HightlightedCode.StartScrollAnimation(scrollTop, scrollDistance, documentScrollY);
            }
            DXDemo.HightlightedCode.visitedTabArrayIndexes[s.GetActiveTabIndex()] = true;
        }
    }
};
function selectorPopupContainerElement_Click(popupControl, event) {
    var eventSource = ASPx.Evt.GetEventSource(event);
    if(eventSource && eventSource.className.indexOf('dxpc-content') >= 0) {
        popupControl.HideWindow();
    }
}
function DXSelectorPopupContainer_Init(sender) {
    var content = sender.GetWindowContentElement(-1);
    ASPxClientUtils.AttachEventToElement(content, 'click', function(event) { selectorPopupContainerElement_Click(sender, event); });
}

window.DXDemo = DXDemo;
window.DXEventMonitor = DXEventMonitor;
window.DXUploadedFilesContainer = DXUploadedFilesContainer;
window.DXSelectorPopupContainer_Init = DXSelectorPopupContainer_Init;

})();
