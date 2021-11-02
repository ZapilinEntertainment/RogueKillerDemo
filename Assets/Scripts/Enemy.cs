using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private Renderer _mainRenderer;
        public Renderer MainRenderer => _mainRenderer;
    }
}
