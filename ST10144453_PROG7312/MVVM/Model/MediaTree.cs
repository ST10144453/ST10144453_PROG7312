using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class MediaTree
    {
        private MediaNode root;
        private Dictionary<string, MediaNode> mediaIndex;

        public MediaTree()
        {
            root = new MediaNode(null);
            mediaIndex = new Dictionary<string, MediaNode>();
        }

        public void AddMedia(MediaItem media, string parentId = null)
        {
            var newNode = new MediaNode(media);
            mediaIndex[media.Id] = newNode;

            if (!string.IsNullOrEmpty(parentId) && mediaIndex.ContainsKey(parentId))
            {
                var parentNode = mediaIndex[parentId];
                newNode.Parent = parentNode;
                parentNode.Children.Add(newNode);
            }
            else
            {
                root.Children.Add(newNode);
            }
        }

        public IEnumerable<MediaItem> GetAllMedia()
        {
            return TraverseTree(root);
        }

        private IEnumerable<MediaItem> TraverseTree(MediaNode node)
        {
            if (node.Data != null)
                yield return node.Data;

            foreach (var child in node.Children)
            {
                foreach (var item in TraverseTree(child))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<MediaItem> GetRelatedMedia(string mediaId)
        {
            if (!mediaIndex.ContainsKey(mediaId))
                return Enumerable.Empty<MediaItem>();

            var node = mediaIndex[mediaId];
            var related = new List<MediaItem>();

            // Get siblings
            if (node.Parent != null)
            {
                related.AddRange(node.Parent.Children
                    .Where(c => c != node)
                    .Select(c => c.Data));
            }

            // Get children
            related.AddRange(node.Children.Select(c => c.Data));

            return related;
        }
    }
}
