"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var Game = function (url) {
  //Configuratie en state waarden
  var configMap = {
    apiUrl: url
  };
  var stateMap = {
    gameState: 0
  };

  var _getCurrentGameState = function _getCurrentGameState() {
    stateMap.gameState = Game.Model.getGameState();
  }; // Private function init


  var privateInit = function privateInit(callbackFunction) {
    // console.log(configMap.apiUrl);
    console.log("Init van Game");
    setInterval(_getCurrentGameState, 2000);
    callbackFunction();
    return true;
  }; // Waarde/object geretourneerd aan de outer scope


  return {
    init: privateInit
  };
}('/api/url');

$(document).ready(function () {
  console.log("ready!");
  $("#btn-ok").on("click", function () {
    alert("The button was clicked.");
    var feedbackWidget = new FeedbackWidget('feedback-danger');
    feedbackWidget.show("Pas op voor loslopende bitsss.", "success");
  });
});

var FeedbackWidget = /*#__PURE__*/function () {
  function FeedbackWidget(elementId) {
    _classCallCheck(this, FeedbackWidget);

    this._elementId = elementId;
  }

  _createClass(FeedbackWidget, [{
    key: "elementId",
    get: function get() {
      return this._elementId;
    }
  }, {
    key: "show",
    value: function show(message, type) {
      var x = document.getElementById(this.elementId);
      x.style.display = "block"; // Show the feedbackwidget

      var elementId = $("#" + this.elementId);
      $(elementId).text(message); // Set the text of the feedback

      if (type === "success") {
        // Check if the type is success, then alert-success
        $(elementId).removeClass('alert-danger');
        $(elementId).addClass('alert-success');
      } else if (type === "danger") {
        // Check if the type is danger, then alert-danger
        $(elementId).removeClass('alert-success');
        $(elementId).addClass('alert-danger');
      }

      this.log({
        message: message,
        type: type
      }); // Call method log and give an object with message and type
    }
  }, {
    key: "hide",
    value: function hide() {
      var x = document.getElementById(this.elementId);
      x.style.display = "none"; // Hide the feedbackwidget
    }
  }, {
    key: "log",
    value: function log(message) {
      var localStorageMessages = JSON.parse(localStorage.getItem('feedback_widget')) || {
        "messages": []
      }; // Get messages from localStorage or make a new JSON object

      localStorageMessages.messages.unshift(message); // Add the message at index 0

      if (localStorageMessages.messages.length > 10) {
        // Check if the messages is bigger then 10
        localStorageMessages.messages.pop(); // Remove the last message from the array
      }

      localStorage.setItem('feedback_widget', JSON.stringify(localStorageMessages)); // Set the localstorage with key 'feedback_widget' and the new message
    }
  }, {
    key: "removeLog",
    value: function removeLog() {
      localStorage.removeItem('feedback_widget'); // Clear the log with key 'feedback_widget'
    }
  }, {
    key: "history",
    value: function history() {
      var localStorageMessages = JSON.parse(localStorage.getItem('feedback_widget')) || {
        "messages": []
      }; // Get messages from localStorage or make a new JSON object

      localStorageMessages.messages.forEach(function (element) {
        return console.log(element.type + " - " + element.message + " ");
      }); // Show the messages in the console
    }
  }]);

  return FeedbackWidget;
}();

Game.Data = function () {
  var configMap = {
    apiKey: "7fa35c872e1d1bf312fc86d652fa4965",
    // OpenWeatherApi key
    mock: [{
      url: "api/Spel/Beurt",
      data: 0
    }]
  };
  var stateMap = {
    environment: 'development'
  };

  var getMockData = function getMockData(url) {
    var mockData = configMap.mock;
    return new Promise(function (resolve, reject) {
      resolve(mockData);
    });
  };

  var get = function get(url) {
    // Returns the data from the OpenWeatherApi
    if (stateMap.environment === "production") {
      return $.get(url + configMap.apiKey).then(function (r) {
        return r;
      })["catch"](function (e) {
        console.log(e.message);
      });
    } else if (stateMap.environment === "development") {
      return getMockData(url);
    }
  };

  var privateInit = function privateInit(environment) {
    stateMap.environment = environment;

    switch (stateMap.environment) {
      case "development":
        break;

      case "production":
        break;

      default:
        throw new Error("Verkeerde environment gegeven");
    }

    console.log("Init van Game Data");
  };

  return {
    init: privateInit,
    get: get
  };
}();

Game.Model = function () {
  var configMap = {};

  var _getGameState = function _getGameState() {
    var gameState; //aanvraag via Game.Data

    Game.Data.get("/api/Spel/Beurt/tokentje").then(function (data) {
      gameState = data[0].data; //controle of ontvangen data valide is

      switch (gameState) {
        case 0:
          console.log("game state heeft geen specifieke waarde");
          break;

        case 1:
          console.log("game state wit aan zet");
          break;

        case 2:
          console.log("game state zwart aan zet");
          break;

        default:
          throw new Error("game state is niet geldig");
      }

      return gameState;
    });
  };

  var privateInit = function privateInit() {
    console.log("Init van Game Model");
  };

  var _getWeather = function _getWeather() {
    var url = "http://api.openweathermap.org/data/2.5/weather?q=zwolle&apikey=";
    Game.Data.get(url).then(function (data) {
      if (data.main.temp === null) {
        console.log("temperatuur niet meegegeven");
      } else {
        console.log("temperatuur meegegeven");
        console.log(data.main.temp);
      }
    });
  };

  return {
    init: privateInit,
    getWeather: _getWeather,
    getGameState: _getGameState
  };
}();

Game.Reversi = function () {
  var configMap = {};

  var privateInit = function privateInit() {
    console.log("Init van Game Reversi");
  };

  return {
    init: privateInit
  };
}();