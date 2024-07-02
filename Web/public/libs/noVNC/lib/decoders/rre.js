"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports["default"] = void 0;
function _typeof(o) { "@babel/helpers - typeof"; return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (o) { return typeof o; } : function (o) { return o && "function" == typeof Symbol && o.constructor === Symbol && o !== Symbol.prototype ? "symbol" : typeof o; }, _typeof(o); }
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }
function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, _toPropertyKey(descriptor.key), descriptor); } }
function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }
function _toPropertyKey(t) { var i = _toPrimitive(t, "string"); return "symbol" == _typeof(i) ? i : i + ""; }
function _toPrimitive(t, r) { if ("object" != _typeof(t) || !t) return t; var e = t[Symbol.toPrimitive]; if (void 0 !== e) { var i = e.call(t, r || "default"); if ("object" != _typeof(i)) return i; throw new TypeError("@@toPrimitive must return a primitive value."); } return ("string" === r ? String : Number)(t); }
/*
 * noVNC: HTML5 VNC client
 * Copyright (C) 2019 The noVNC Authors
 * Licensed under MPL 2.0 (see LICENSE.txt)
 *
 * See README.md for usage and integration instructions.
 *
 */
var RREDecoder = exports["default"] = /*#__PURE__*/function () {
  function RREDecoder() {
    _classCallCheck(this, RREDecoder);
    this._subrects = 0;
  }
  return _createClass(RREDecoder, [{
    key: "decodeRect",
    value: function decodeRect(x, y, width, height, sock, display, depth) {
      if (this._subrects === 0) {
        if (sock.rQwait("RRE", 4 + 4)) {
          return false;
        }
        this._subrects = sock.rQshift32();
        var color = sock.rQshiftBytes(4); // Background
        display.fillRect(x, y, width, height, color);
      }
      while (this._subrects > 0) {
        if (sock.rQwait("RRE", 4 + 8)) {
          return false;
        }
        var _color = sock.rQshiftBytes(4);
        var sx = sock.rQshift16();
        var sy = sock.rQshift16();
        var swidth = sock.rQshift16();
        var sheight = sock.rQshift16();
        display.fillRect(x + sx, y + sy, swidth, sheight, _color);
        this._subrects--;
      }
      return true;
    }
  }]);
}();