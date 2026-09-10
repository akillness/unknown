using System;
using System.Collections.Generic;
using Tide.Data;
using Tide.Sim;
using UnityEngine;

namespace Tide.App
{
    public sealed class T0Bootstrap : MonoBehaviour
    {
        [SerializeField] private T0CatalogAsset catalog;
        public PuzzleState Snapshot { get; private set; }
        public IReadOnlyList<string> BlockingDataIssues { get; private set; }
        public bool IsReady { get; private set; }
        public void Initialize(T0CatalogAsset source)
        {
            var definition=source.Load();
            BlockingDataIssues=T0DataLoader.CitationBlockers(definition);
            IsReady=BlockingDataIssues.Count==0;
            // A full T0 session must not start with a known unreachable citation completion.
            Snapshot=IsReady?new PuzzleState():null;
        }
        private void Awake()
        {
            if(catalog!=null) Initialize(catalog);
        }
    }
}

