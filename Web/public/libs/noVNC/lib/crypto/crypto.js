"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports["default"] = void 0;
var _aes = require("./aes.js");
var _des = require("./des.js");
var _rsa = require("./rsa.js");
var _dh = require("./dh.js");
var _md = require("./md5.js");
function _typeof(o) { "@babel/helpers - typeof"; return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (o) { return typeof o; } : function (o) { return o && "function" == typeof Symbol && o.constructor === Symbol && o !== Symbol.prototype ? "symbol" : typeof o; }, _typeof(o); }
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }
function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, _toPropertyKey(descriptor.key), descriptor); } }
function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }
function _toPropertyKey(t) { var i = _toPrimitive(t, "string"); return "symbol" == _typeof(i) ? i : i + ""; }
function _toPrimitive(t, r) { if ("object" != _typeof(t) || !t) return t; var e = t[Symbol.toPrimitive]; if (void 0 !== e) { var i = e.call(t, r || "default"); if ("object" != _typeof(i)) return i; throw new TypeError("@@toPrimitive must return a primitive value."); } return ("string" === r ? String : Number)(t); }
// A single interface for the cryptographic algorithms not supported by SubtleCrypto.
// Both synchronous and asynchronous implmentations are allowed.
var LegacyCrypto = /*#__PURE__*/function () {
  function LegacyCrypto() {
    _classCallCheck(this, LegacyCrypto);
    this._algorithms = {
      "AES-ECB": _aes.AESECBCipher,
      "AES-EAX": _aes.AESEAXCipher,
      "DES-ECB": _des.DESECBCipher,
      "DES-CBC": _des.DESCBCCipher,
      "RSA-PKCS1-v1_5": _rsa.RSACipher,
      "DH": _dh.DHCipher,
      "MD5": _md.MD5
    };
  }
  return _createClass(LegacyCrypto, [{
    key: "encrypt",
    value: function encrypt(algorithm, key, data) {
      if (key.algorithm.name !== algorithm.name) {
        throw new Error("algorithm does not match");
      }
      if (typeof key.encrypt !== "function") {
        throw new Error("key does not support encryption");
      }
      return key.encrypt(algorithm, data);
    }
  }, {
    key: "decrypt",
    value: function decrypt(algorithm, key, data) {
      if (key.algorithm.name !== algorithm.name) {
        throw new Error("algorithm does not match");
      }
      if (typeof key.decrypt !== "function") {
        throw new Error("key does not support encryption");
      }
      return key.decrypt(algorithm, data);
    }
  }, {
    key: "importKey",
    value: function importKey(format, keyData, algorithm, extractable, keyUsages) {
      if (format !== "raw") {
        throw new Error("key format is not supported");
      }
      var alg = this._algorithms[algorithm.name];
      if (typeof alg === "undefined" || typeof alg.importKey !== "function") {
        throw new Error("algorithm is not supported");
      }
      return alg.importKey(keyData, algorithm, extractable, keyUsages);
    }
  }, {
    key: "generateKey",
    value: function generateKey(algorithm, extractable, keyUsages) {
      var alg = this._algorithms[algorithm.name];
      if (typeof alg === "undefined" || typeof alg.generateKey !== "function") {
        throw new Error("algorithm is not supported");
      }
      return alg.generateKey(algorithm, extractable, keyUsages);
    }
  }, {
    key: "exportKey",
    value: function exportKey(format, key) {
      if (format !== "raw") {
        throw new Error("key format is not supported");
      }
      if (typeof key.exportKey !== "function") {
        throw new Error("key does not support exportKey");
      }
      return key.exportKey();
    }
  }, {
    key: "digest",
    value: function digest(algorithm, data) {
      var alg = this._algorithms[algorithm];
      if (typeof alg !== "function") {
        throw new Error("algorithm is not supported");
      }
      return alg(data);
    }
  }, {
    key: "deriveBits",
    value: function deriveBits(algorithm, key, length) {
      if (key.algorithm.name !== algorithm.name) {
        throw new Error("algorithm does not match");
      }
      if (typeof key.deriveBits !== "function") {
        throw new Error("key does not support deriveBits");
      }
      return key.deriveBits(algorithm, length);
    }
  }]);
}();
var _default = exports["default"] = new LegacyCrypto();