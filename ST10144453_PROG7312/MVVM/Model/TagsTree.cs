using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class TagsTree
    {
        private TagNode root;
        private Dictionary<int, TagNode> tagIndex;

        public TagsTree()
        {
            root = new TagNode(null); // Root node is empty
            tagIndex = new Dictionary<int, TagNode>();
        }

        public void AddTag(TagsModel tag, int? parentTagId = null)
        {
            var newNode = new TagNode(tag);
            tagIndex[tag.TagId] = newNode;

            if (parentTagId.HasValue && tagIndex.ContainsKey(parentTagId.Value))
            {
                var parentNode = tagIndex[parentTagId.Value];
                newNode.Parent = parentNode;
                parentNode.Children.Add(newNode);
            }
            else
            {
                root.Children.Add(newNode);
            }
        }

        public IEnumerable<TagsModel> GetRelatedTags(int tagId, int depth = 1)
        {
            if (!tagIndex.ContainsKey(tagId)) yield break;

            var node = tagIndex[tagId];
            var visited = new HashSet<int>();

            foreach (var relatedTag in GetRelatedTagsRecursive(node, depth, visited))
            {
                yield return relatedTag;
            }
        }

        private IEnumerable<TagsModel> GetRelatedTagsRecursive(TagNode node, int depth, HashSet<int> visited)
        {
            if (depth < 0 || node == null || visited.Contains(node.Data.TagId)) yield break;

            visited.Add(node.Data.TagId);

            foreach (var child in node.Children)
            {
                yield return child.Data;
                foreach (var descendant in GetRelatedTagsRecursive(child, depth - 1, visited))
                {
                    yield return descendant;
                }
            }

            if (node.Parent != null && node.Parent.Data != null)
            {
                yield return node.Parent.Data;
                foreach (var sibling in node.Parent.Children.Where(c => c != node))
                {
                    yield return sibling.Data;
                }
            }
        }
    }
}
