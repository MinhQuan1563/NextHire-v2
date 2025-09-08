using System;
using System.Threading.Tasks;

namespace NextHireApp.Service
{
    /// <summary>
    /// Service quản lý <b>deny-list</b> cho JWT nhằm thu hồi (revoke) token sau khi phát hành.
    /// </summary>
    /// <remarks>
    /// Lưu <c>jti</c> (JWT ID) vào storage (Redis) với thời hạn <paramref name="ttl"/>.
    /// Dùng cho: logout chủ động, force-revoke khi phát hiện rủi ro, hoặc rotate refresh token thất bại.
    /// Khuyến nghị: <b>TTL = thời gian còn lại của token</b> (hoặc dài nhất bằng thời hạn refresh token).
    /// </remarks>
    public interface ITokenBlacklistService
    {
        /// <summary>
        /// Đưa một JWT vào danh sách chặn (deny-list) trong thời hạn chỉ định.
        /// </summary>
        /// <param name="jti">Giá trị <c>JWT ID</c> (claim <c>jti</c>) của token cần chặn.</param>
        /// <param name="ttl">Thời gian hiệu lực của việc chặn; hết hạn sẽ tự gỡ.</param>
        /// <returns>Nhiệm vụ bất đồng bộ.</returns>
        Task BlacklistAsync(string jti, TimeSpan ttl);

        /// <summary>
        /// Kiểm tra token (qua <c>jti</c>) có đang bị chặn hay không.
        /// </summary>
        /// <param name="jti">Giá trị <c>JWT ID</c> cần kiểm tra.</param>
        /// <returns>
        /// <see langword="true"/> nếu token đang trong deny-list; ngược lại <see langword="false"/>.
        /// </returns>
        Task<bool> IsBlacklistedAsync(string jti);
    }
}
