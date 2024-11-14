using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class AnnouncementTree
    {
        private AnnouncementNode root;

        public AnnouncementTree()
        {
            root = null;
        }

        private int GetHeight(AnnouncementNode node)
        {
            return node?.Height ?? 0;
        }

        private int GetBalance(AnnouncementNode node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private AnnouncementNode RightRotate(AnnouncementNode y)
        {
            AnnouncementNode x = y.Left;
            AnnouncementNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        private AnnouncementNode LeftRotate(AnnouncementNode x)
        {
            AnnouncementNode y = x.Right;
            AnnouncementNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        public void Insert(AnnouncementModel announcement)
        {
            if (announcement == null)
                throw new ArgumentNullException(nameof(announcement));

            root = InsertRec(root, announcement);
        }

        private AnnouncementNode InsertRec(AnnouncementNode node, AnnouncementModel announcement)
        {
            if (node == null)
                return new AnnouncementNode(announcement);

            if (announcement.announcementDate.CompareTo(node.Data.announcementDate) < 0)
                node.Left = InsertRec(node.Left, announcement);
            else if (announcement.announcementDate.CompareTo(node.Data.announcementDate) > 0)
                node.Right = InsertRec(node.Right, announcement);
            else
                return node;

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Left Left Case
            if (balance > 1 && announcement.announcementDate.CompareTo(node.Left.Data.announcementDate) < 0)
                return RightRotate(node);

            // Right Right Case
            if (balance < -1 && announcement.announcementDate.CompareTo(node.Right.Data.announcementDate) > 0)
                return LeftRotate(node);

            // Left Right Case
            if (balance > 1 && announcement.announcementDate.CompareTo(node.Left.Data.announcementDate) > 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // Right Left Case
            if (balance < -1 && announcement.announcementDate.CompareTo(node.Right.Data.announcementDate) < 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        public IEnumerable<AnnouncementModel> GetAnnouncementsInOrder()
        {
            var announcements = new List<AnnouncementModel>();
            InOrderTraversal(root, announcements);
            return announcements;
        }

        private void InOrderTraversal(AnnouncementNode node, List<AnnouncementModel> announcements)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, announcements);
                announcements.Add(node.Data);
                InOrderTraversal(node.Right, announcements);
            }
        }

        public IEnumerable<AnnouncementModel> GetRecentAnnouncements(int count)
        {
            var announcements = new List<AnnouncementModel>();
            GetRecentAnnouncementsRec(root, announcements, count, DateTime.Now);
            return announcements.OrderByDescending(a => a.announcementDate).Take(count);
        }

        private void GetRecentAnnouncementsRec(AnnouncementNode node, List<AnnouncementModel> announcements, int count, DateTime currentDate)
        {
            if (node == null || announcements.Count >= count)
                return;

            GetRecentAnnouncementsRec(node.Right, announcements, count, currentDate);
            
            if (announcements.Count < count)
            {
                announcements.Add(node.Data);
                GetRecentAnnouncementsRec(node.Left, announcements, count, currentDate);
            }
        }
    }
}
